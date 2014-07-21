using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Anotar.CommonLogging;
using CQRSMagic.Event;
using CQRSMagic.EventStorage;
using CQRSMagic.Support;

namespace CQRSMagic.Command
{
    public class CommandBus : ICommandBus
    {
        private readonly ICommandHandlers CommandHandlers;
        private readonly IEventBus EventBus;
        private readonly IEventStore EventStore;

        public CommandBus()
            : this(IoC.Get<IEventStore>(), IoC.Get<IEventBus>(), IoC.Get<ICommandHandlers>())
        {
        }

        public CommandBus(IEventStore eventStore, IEventBus eventBus, ICommandHandlers commandHandlers)
        {
            EventStore = eventStore;
            EventBus = eventBus;
            CommandHandlers = commandHandlers;
        }

        public async Task<IEnumerable<Task>> SendCommandAsync(ICommand command)
        {
            var events = (await GetEvents(command)).ToArray();

            // EventBus subscriptions often use IEventStore.GetAggregate. Therefore IEventStore.SaveEventsAsync must complete first.
            var saveEventsTask = EventStore.SaveEventsAsync(events);
            var sendEventsTask = saveEventsTask.ContinueWith(continuation =>
            {
                if (continuation.Status == TaskStatus.RanToCompletion)
                {
                    EventBus.SendEventsAsync(events);
                }
                else
                {
                    LogTo.Warn("EventBus.SendEventsAsync(events) was not called because EventStore.SaveEventsAsync(events) status is {0}.", continuation.Status);
                }
            });

            return new[] {saveEventsTask, sendEventsTask};
        }

        public void RegisterHandler<TCommand>(Func<TCommand, Task<IEnumerable<IEvent>>> handler) where TCommand : ICommand
        {
            CommandHandlers.RegisterHandler(handler);
        }

        public void RegisterHandlers(Assembly searchAssembly)
        {
            CommandHandlers.RegisterHandlers(searchAssembly);
        }

        private Task<IEnumerable<IEvent>> GetEvents(ICommand command)
        {
            var handler = CommandHandlers.GetHandler(command);
            var events = handler(command);

            return events;
        }
    }
}