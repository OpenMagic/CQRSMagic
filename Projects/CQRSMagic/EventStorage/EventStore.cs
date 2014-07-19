using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CQRSMagic.Domain;
using CQRSMagic.Event;
using CQRSMagic.Support;
using Microsoft.Practices.ServiceLocation;

namespace CQRSMagic.EventStorage
{
    public class EventStore : IEventStore
    {
        private readonly IEventStoreRepository Repository;
        private readonly IServiceLocator Services;

        public EventStore()
            : this(IoC.Get<IEventStoreRepository>())
        {
        }

        public EventStore(IEventStoreRepository repository)
            : this(repository, IoC.Get<IServiceLocator>())
        {
        }

        public EventStore(IEventStoreRepository repository, IServiceLocator serviceLocator)
        {
            Repository = repository;
            Services = serviceLocator;
        }

        public async Task<IEnumerable<IEvent>> FindEventsAsync(Guid aggregateId)
        {
            return await Repository.FindEventsAsync(aggregateId);
        }

        public async Task<TAggregate> GetAggregateAsync<TAggregate>(Guid aggregateId) where TAggregate : IAggregate
        {
            var events = await Repository.FindEventsAsync(aggregateId);
            var aggregate = Services.Get<TAggregate>();

            aggregate.ApplyEvents(events);

            return aggregate;
        }

        public Task SaveEventsAsync(IEnumerable<IEvent> events)
        {
            return Repository.SaveEventsAsync(events);
        }
    }
}