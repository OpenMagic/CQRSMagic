using System;
using System.Collections.Generic;
using System.Linq;
using Anotar.CommonLogging;
using CQRSMagic.Domain;
using CQRSMagic.Events.Messaging;
using CQRSMagic.Events.Sourcing.Repositories;

namespace CQRSMagic.Events.Sourcing
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
            LogTo.Debug("GetAggregate<{0}>({1})", typeof(TAggregate), aggregateId);

            LogTo.Debug("Repository.GetEvents<{0}>({1})", typeof(TAggregate), aggregateId);
            var events = Repository.GetEvents<TAggregate>(aggregateId);

            LogTo.Debug("CreateInstance<{0}>()", typeof(TAggregate));
            var aggregate = Activator.CreateInstance<TAggregate>();

            LogTo.Debug("{0}.ApplyEvents(events)", typeof(TAggregate));
            aggregate.ApplyEvents(events);

            LogTo.Debug("Exit GetAggregate<{0}>({1})", typeof(TAggregate), aggregateId);
            return aggregate;
        }

        public void SaveEvents(IEnumerable<IEvent> events)
        {
            SaveEvents(events.ToArray());
        }

        private void SaveEvents(IEvent[] events)
        {
            LogTo.Debug("SaveEvents(IEvent[{0}] events", events.Length);
            Repository.SaveEvents(events);
            LogTo.Debug("Exit - SaveEvents(IEvent[{0}] events", events.Length);
        }
    }
}