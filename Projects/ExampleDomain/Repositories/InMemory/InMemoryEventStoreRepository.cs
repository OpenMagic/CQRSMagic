using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRSMagic.Event;
using CQRSMagic.EventStorage;

namespace ExampleDomain.Repositories.InMemory
{
    public class InMemoryEventStoreRepository : IEventStoreRepository
    {
        private readonly ConcurrentDictionary<Guid, IEnumerable<IEvent>> Events;

        public InMemoryEventStoreRepository()
        {
            Events = new ConcurrentDictionary<Guid, IEnumerable<IEvent>>();
        }

        public Task<IEnumerable<IEvent>> FindAllEventsAsync()
        {
            return Task.FromResult(
                from aggregateEvents in Events
                from events in aggregateEvents.Value
                select events);
        }

        public Task<IEnumerable<IEvent>> FindEventsAsync(Guid aggregateId)
        {
            return Task.FromResult(Events[aggregateId]);
        }

        public Task SaveEventsAsync(IEnumerable<IEvent> events)
        {
            foreach (var @event in events)
            {
                var aggregateId = @event.AggregateId;

                Events.AddOrUpdate(aggregateId, id => new[] {@event}, (id, currentEvents) => AddEvent(currentEvents, @event));
            }

            return Task.FromResult(true);
        }

        private static IEnumerable<IEvent> AddEvent(IEnumerable<IEvent> currentEvents, IEvent @event)
        {
            var newEvents = currentEvents.ToList();

            newEvents.Add(@event);

            return newEvents;
        }
    }
}