using System.Collections.Generic;
using System.Threading.Tasks;
using CQRSMagic.Events.Messaging;

namespace CQRSMagic.Events.Publishing
{
    public class EventPublisher : IEventPublisher
    {
        private readonly ISubscriptionHandlers Subscriptions;

        public EventPublisher(ISubscriptionHandlers subscriptions)
        {
            Subscriptions = subscriptions;
        }

        public async Task PublishEventsAsync(IEnumerable<IEvent> events)
        {
            // todo: unit tests

            foreach (var @event in events)
            {
                // todo: will this stop the loop?
                await PublishEventAsync(@event);
            }
        }

        private async Task PublishEventAsync(IEvent @event)
        {
            var subscriptions = Subscriptions.FindSubscriptionsFor(@event);

            foreach (var subscription in subscriptions)
            {
                // todo: will this stop the loop?
                await subscription(@event);
            }
        }
    }
}