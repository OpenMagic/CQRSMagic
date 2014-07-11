using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<IEvent>> SendCommandAsync(ICommand command)
        {
            // todo: unit tests
            var eventCollection = await CommandBus.SendCommandAsync(command);
            var events = eventCollection.ToArray();

            await EventBus.SendEventsAsync(events);
            
#pragma warning disable 4014
            EventPublisher.PublishEventsAsync(events);
#pragma warning restore 4014

            return events;
        }
    }
}