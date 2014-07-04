using System;
using System.Collections.Generic;
using CQRSMagic.Domain;

namespace CQRSMagic.Events
{
    public class EventStore : IEventStore
    {
        private readonly IEventStoreRepository Repository;

        public EventStore(IEventStoreRepository repository)
        {
            // todo: unit tests
            Repository = repository;
        }

        public TAggregate GetAggregate<TAggregate>(Guid aggregateId) where TAggregate : IAggregate
        {
            // todo: unit tests

            var events = Repository.GetEvents<TAggregate>(aggregateId);

            // todo: use container
            var aggregate = Activator.CreateInstance<TAggregate>();

            aggregate.ApplyEvents(events);

            return aggregate;
        }

        public void SaveEvents(IEnumerable<IEvent> events)
        {
            // todo: unit tests
            Repository.SaveEvents(events);
        }
    }
}