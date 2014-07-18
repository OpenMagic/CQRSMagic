using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CQRSMagic.Domain;
using CQRSMagic.Event;
using CQRSMagic.IoC;

namespace CQRSMagic.EventStorage
{
    public class EventStore : IEventStore
    {
        private readonly IDependencyResolver DependencyResolver;
        private readonly IEventStoreRepository Repository;

        public EventStore(IEventStoreRepository repository, IDependencyResolver dependencyResolver)
        {
            Repository = repository;
            DependencyResolver = dependencyResolver;
        }

        public async Task<IEnumerable<IEvent>> FindEventsAsync(Guid aggregateId)
        {
            return await Repository.FindEventsAsync(aggregateId);
        }

        public async Task<TAggregate> GetAggregateAsync<TAggregate>(Guid aggregateId) where TAggregate : IAggregate
        {
            var events = await Repository.FindEventsAsync(aggregateId);
            var aggregate = DependencyResolver.Get<TAggregate>();

            aggregate.ApplyEvents(events);

            return aggregate;
        }

        public Task SaveEventsAsync(IEnumerable<IEvent> events)
        {
            return Repository.SaveEventsAsync(events);
        }
    }
}