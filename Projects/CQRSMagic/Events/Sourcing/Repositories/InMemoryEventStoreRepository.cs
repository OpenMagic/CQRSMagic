using System;
using System.Collections.Generic;
using CQRSMagic.Domain;
using CQRSMagic.Exceptions;

namespace CQRSMagic.Events
{
    public class InMemoryEventStoreRepository : IEventStoreRepository
    {
        private readonly Dictionary<Guid, List<IEvent>> AggregateEvents = new Dictionary<Guid, List<IEvent>>();

        public IEnumerable<IEvent> GetEvents<TAggregate>(Guid aggregateId) where TAggregate : IAggregate
        {
            List<IEvent> events;

            if (AggregateEvents.TryGetValue(aggregateId, out events))
            {
                return events;
            }

            throw new AggregateNotFoundException<TAggregate>(aggregateId);
        }

        public void SaveEvents(IEnumerable<IEvent> newEvents)
        {
            foreach (var newEvent in newEvents)
            {
                SaveEvent(newEvent);
            }
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
