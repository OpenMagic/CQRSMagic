using System.Collections.Generic;
using System.Linq;
using CQRSMagic.Commands;
using CQRSMagic.Events.Messaging;
using CQRSMagic.Events.Publishing;

namespace CQRSMagic
{
    public class MessageBus : IMessageBus
    {
        private readonly ICommandBus CommandBus;
        private readonly IEventBus EventBus;
        private readonly IEventPublisher EventPublisher;

        public MessageBus(ICommandBus commandBus, IEventBus eventBus, IEventPublisher eventPublisher)
        {
            // todo: unit tests
            CommandBus = commandBus;
            EventBus = eventBus;
            EventPublisher = eventPublisher;
        }

        public IEnumerable<IEvent> SendCommand(ICommand command)
        {
            // todo: unit tests
            var events = CommandBus.SendCommand(command).ToArray();

            EventBus.SendEvents(events);
            EventPublisher.PublishEventsAsync(events);

            return events;
        }
    }
}