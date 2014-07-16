using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRSMagic.Event;
using CQRSMagic.EventStorage;

namespace CQRSMagic.Command
{
    public class CommandBus : ICommandBus
    {
        private readonly IEventBus EventBus;
        private readonly IEventStore EventStore;
        private readonly Dictionary<Type, Func<ICommand, Task<IEnumerable<IEvent>>>> Handlers;

        public CommandBus(IEventStore eventStore, IEventBus eventBus)
        {
            EventStore = eventStore;
            EventBus = eventBus;
            Handlers = new Dictionary<Type, Func<ICommand, Task<IEnumerable<IEvent>>>>();
        }

        public async Task SendCommandAsync(ICommand command)
        {
            var tasks = new List<Task>();
            var events = (await GetEvents(command)).ToArray();

            tasks.Add(EventStore.SaveEventsAsync(events));
            tasks.Add(EventBus.SendEventsAsync(events));
        }

        public void RegisterHandler<TCommand>(Func<TCommand, Task<IEnumerable<IEvent>>> handler) where TCommand : ICommand
        {
            var key = typeof(TCommand);

            if (Handlers.ContainsKey(key))
            {
                throw new DuplicateCommandHandlerException(key);
            }

            Func<ICommand, Task<IEnumerable<IEvent>>> value = command => handler((TCommand) command);

            Handlers.Add(key, value);
        }

        private Task<IEnumerable<IEvent>> GetEvents(ICommand command)
        {
            var handler = GetHandler(command);
            var events = handler(command);

            return events;
        }

        private Func<ICommand, Task<IEnumerable<IEvent>>> GetHandler(ICommand command)
        {
            Func<ICommand, Task<IEnumerable<IEvent>>> handler = null;

            if (Handlers.TryGetValue(command.GetType(), out handler))
            {
                return handler;
            }

            throw new CommandHandlerNotFoundException(command);
        }
    }
}