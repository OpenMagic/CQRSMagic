using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRSMagic.Domain;
using CQRSMagic.Events.Messaging;
using CQRSMagic.Exceptions;

namespace CQRSMagic.Events.Sourcing.Repositories
{
    public class InMemoryEventStoreRepository : IEventStoreRepository
    {
        private readonly Dictionary<Guid, List<IEvent>> AggregateEvents = new Dictionary<Guid, List<IEvent>>();

        public Task<IEnumerable<IEvent>> GetEventsAsync<TAggregate>(Guid aggregateId) where TAggregate : IAggregate
        {
            List<IEvent> events;

            if (AggregateEvents.TryGetValue(aggregateId, out events))
            {
                return Task.FromResult(events.AsEnumerable());
            }

            throw new AggregateNotFoundException<TAggregate>(aggregateId);
        }

        public Task SaveEventsAsync(IEnumerable<IEvent> newEvents)
        {
            foreach (var newEvent in newEvents)
            {
                SaveEvent(newEvent);
            }

            return Task.FromResult(0);
        }

        private void SaveEvent(IEvent @event)
        {
            List<IEvent> events;

            if (!AggregateEvents.TryGetValue(@event.AggregateId, out events))
            {
                events = new List<IEvent>();
                AggregateEvents.Add(@event.AggregateId, events);
            }

            events.Add(@event);
        }
    }
}