using System;
using System.Collections.Generic;
using System.Linq;
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
            var events = GetEvents<TAggregate>(aggregateId);
            var aggregate = CreateInstance<TAggregate>();
            aggregate.ApplyEvents(events);
            return aggregate;
        }

        public void SaveEvents(IEnumerable<IEvent> events)
        {
            SaveEvents(events.ToArray());
        }

        private static TAggregate CreateInstance<TAggregate>() where TAggregate : IAggregate
        {
            try
            {
                return Activator.CreateInstance<TAggregate>();
            }
            catch (Exception exception)
            {
                var message = string.Format("Cannot create instance of {0}.", typeof(TAggregate));
                throw new Exception(message, exception);
            }
        }

        private IEnumerable<IEvent> GetEvents<TAggregate>(Guid aggregateId) where TAggregate : IAggregate
        {
            try
            {
                return Repository.GetEvents<TAggregate>(aggregateId);
            }
            catch (Exception exception)
            {
                var message = string.Format("Cannot get events for {0}.", typeof(TAggregate));
                throw new Exception(message, exception);
            }
        }

        private void SaveEvents(IEvent[] events)
        {
            try
            {
                Repository.SaveEvents(events);
            }
            catch (Exception exception)
            {
                const string message = "Cannot save events.";
                throw new Exception(message, exception);
            }
        }
    }
}