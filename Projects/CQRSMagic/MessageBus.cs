using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CQRSMagic
{
    public class MessageBus : IMessageBus
    {
        private readonly ICommandHandlers CommandHandlers;
        private readonly IEventStore EventStore;
        private readonly ISubscribeToHandlers SubscribeToHandlers;

        public MessageBus(IEventStore eventStore, ICommandHandlers commandHandlers, ISubscribeToHandlers subscribeToHandlers)
        {
            EventStore = eventStore;
            CommandHandlers = commandHandlers;
            SubscribeToHandlers = subscribeToHandlers;
        }

        /// <summary>
        ///     Sends the command and passes raised events to event store and subscribers to those events.
        /// </summary>
        /// <param name="command">
        ///     The command to send.
        /// </param>
        /// <returns>
        ///     <see cref="SendCommand" /> fires three tasks:
        ///     - SendCommand: This task sends <paramref name="command" /> to its registered handler for processing. This task is
        ///     the most likely you will wait for a result on.
        ///     -
        ///     Each of these tasks are returned via <see cref="ISendCommandTasks" />.
        /// </returns>
        public ISendCommandTasks SendCommand(ICommand command)
        {
            IEnumerable<IEvent> eventsResult = null;

            var commandHandler = CommandHandlers.GetCommandHandler(command);
            var events = commandHandler.SendCommand(command, EventStore);
            var tasks = new SendCommandTasks {SendCommand = events};

            tasks.SendCommand.ContinueWith(continuation =>
            {
                if (continuation.Status != TaskStatus.RanToCompletion)
                {
                    return;
                }

                eventsResult = continuation.Result;

                tasks.EventStore = EventStore.SaveEventsFor(commandHandler.AggregateType, command.AggregateId, eventsResult);
            });

            if (tasks.EventStore == null)
            {
                return tasks;
            }

            tasks.EventStore.ContinueWith(continuation =>
            {
                if (continuation.Status != TaskStatus.RanToCompletion)
                {
                    return;
                }

                if (eventsResult == null)
                {
                    throw new InvalidOperationException("eventsResults should have been initialized by task that runs EventStore.SaveEventsFor.");
                }

                tasks.Subscriptions = SubscribeToHandlers.PublishEvents(command.AggregateId, eventsResult);
            });

            return tasks;
        }
    }
}