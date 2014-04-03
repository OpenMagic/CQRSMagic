using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Library.CQRS.Exceptions;

namespace Library.CQRS
{
    public class EventStore : IEventStore
    {
        private readonly IEventStoreRepository Repository;

        public EventStore(IEventStoreRepository repository)
        {
            Repository = repository;
        }

        public async Task<TAggregate> GetAggregate<TAggregate>(Guid aggregateId) where TAggregate : IAggregate, new()
        {
            var aggregate = new TAggregate();
            var events = await Repository.FindEventsFor(typeof(TAggregate), aggregateId);

            if (events == null)
            {
                throw new AggregateNotFoundException(typeof(TAggregate), aggregateId);
            }

            await aggregate.SendEvents(events);

            return aggregate;
        }

        public async Task<IEnumerable<IEvent>> GetEventsFor<TAggregate>(Guid aggregateId) where TAggregate : IAggregate
        {
            var events = await Repository.FindEventsFor(typeof(TAggregate), aggregateId);

            if (events == null)
            {
                throw new EventsNotFoundException(typeof(TAggregate), aggregateId);
            }

            return events;
        }

        public async Task SaveEventsFor(Type aggregateType, Guid aggregateId, IEnumerable<IEvent> events)
        {
            await Repository.SaveEventsFor(aggregateType, aggregateId, events);
        }
    }
}