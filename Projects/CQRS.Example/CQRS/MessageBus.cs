using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Example.CQRS.Commands;
using CQRS.Example.CQRS.Events;
using CQRS.Example.Customers.Commands;
using CQRS.Example.Customers.Domain;

namespace CQRS.Example.CQRS
{
    public class MessageBus : IMessageBus
    {
        private readonly IEventStore EventStore;
        private readonly ICommandSender CommandSender;
        private readonly IEventPublisher EventPublisher;

        public MessageBus(IEventStore eventStore, ICommandSender commandSender, IEventPublisher eventPublisher)
        {
            EventStore = eventStore;
            CommandSender = commandSender;
            EventPublisher = eventPublisher;
        }

        public ISendCommandTasks SendCommand(ICommand command)
        {
            var handleCommand = CommandSender.SendCommand(command);
            var saveEvents = handleCommand.ContinueWith(SaveEvents);
            var publishEvents = saveEvents.ContinueWith(task => PublishEvents(handleCommand, task));

            return new SendCommandTasks(handleCommand, saveEvents, publishEvents);
        }

        private void SaveEvents(Task<IEnumerable<IEvent>> handleCommand)
        {
            if (handleCommand.Status != TaskStatus.RanToCompletion)
            {
                return;
            }
            EventStore.SaveEvents(handleCommand.Result).Wait();
        }

        private void PublishEvents(Task<IEnumerable<IEvent>> handleCommand, Task saveEvents)
        {
            if (handleCommand.Status != TaskStatus.RanToCompletion && saveEvents.Status != TaskStatus.RanToCompletion)
            {
                return;
            }
            Task.WaitAll(EventPublisher.PublishEvents(handleCommand.Result).ToArray());
        }
    }
}