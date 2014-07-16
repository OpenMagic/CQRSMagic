using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CQRSMagic.Events.Messaging;
using CQRSMagic.Events.Publishing;
using CQRSMagic.Exceptions;

namespace CQRSMagic.Specifications.Support
{
    public class SyncEventPublisher : IEventPublisher
    {
        private readonly EventPublisher EventPublisher;

        public SyncEventPublisher(ISubscriptionHandlers subscriptions)
        {
            EventPublisher = new EventPublisher(subscriptions);
        }

        public Task PublishEventsAsync(IEnumerable<IEvent> events)
        {
            foreach (var @event in events)
            {
                EventPublisher.PublishEventAsync(@event).Wait();
            }

            return Task.FromResult(0);
        }
    }
}
