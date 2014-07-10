using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CQRSMagic.Events.Messaging;
using CQRSMagic.Exceptions;

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
            try
            {
                var subscriptions = Subscriptions.FindSubscriptionsFor(@event);

                foreach (var subscription in subscriptions)
                {
                    try
                    {
                        // todo: will this stop the loop?
                        await subscription(@event);
                    }
                    catch (Exception exception)
                    {
                        var message = string.Format("Cannot publish {0} event for {1}/{2} with {3}.{4}.", @event.GetType(), @event.AggregateType, @event.AggregateId, subscription.Method.DeclaringType, subscription.Method.Name);
                        throw new EventException(message, exception);
                    }
                }
            }
            catch (Exception exception)
            {
                var message = string.Format("Cannot publish {0} event for {1}/{2}.", @event.GetType(), @event.AggregateType, @event.AggregateId);
                throw new EventException(message, exception);
            }
        }
    }
}