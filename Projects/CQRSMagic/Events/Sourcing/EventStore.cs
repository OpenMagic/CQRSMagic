using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<TAggregate> GetAggregateAsync<TAggregate>(Guid aggregateId) where TAggregate : IAggregate
        {
            var events = await GetEventsAsync<TAggregate>(aggregateId);
            var aggregate = CreateInstance<TAggregate>();

            aggregate.ApplyEvents(events);

            return aggregate;
        }

        public async Task SaveEventsAsync(IEnumerable<IEvent> events)
        {
            await SaveEventsAsync(events.ToArray());
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

        private async Task<IEnumerable<IEvent>> GetEventsAsync<TAggregate>(Guid aggregateId) where TAggregate : IAggregate
        {
            try
            {
                return await Repository.GetEventsAsync<TAggregate>(aggregateId);
            }
            catch (Exception exception)
            {
                var message = string.Format("Cannot get events for {0}.", typeof(TAggregate));
                throw new Exception(message, exception);
            }
        }

        private async Task SaveEventsAsync(IEvent[] events)
        {
            try
            {
                await Repository.SaveEventsAsync(events);
            }
            catch (Exception exception)
            {
                const string message = "Cannot save events.";
                throw new Exception(message, exception);
            }
        }
    }
}