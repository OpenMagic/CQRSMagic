using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CQRSMagic.Domain;
using CQRSMagic.Event;

namespace CQRSMagic.EventStorage
{
    public class EventStore : IEventStore
    {
        private readonly IAggregateFactory AggregateFactory;
        private readonly IEventStoreRepository Repository;

        public EventStore(IEventStoreRepository repository, IAggregateFactory aggregateFactory)
        {
            Repository = repository;
            AggregateFactory = aggregateFactory;
        }

        public async Task<IEnumerable<IEvent>> FindEventsAsync(Guid aggregateId)
        {
            return await Repository.FindEventsAsync(aggregateId);
        }

        public async Task<TAggregate> GetAggregateAsync<TAggregate>(Guid aggregateId) where TAggregate : IAggregate
        {
            var events = await Repository.FindEventsAsync(aggregateId);
            var aggregate = AggregateFactory.CreateInstance<TAggregate>();

            aggregate.ApplyEvents(events);

            return aggregate;
        }

        public Task SaveEventsAsync(IEnumerable<IEvent> events)
        {
            return Repository.SaveEventsAsync(events);
        }
    }
}