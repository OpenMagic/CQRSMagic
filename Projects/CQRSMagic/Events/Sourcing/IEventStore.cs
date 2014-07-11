using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CQRSMagic.Domain;
using CQRSMagic.Events.Messaging;

namespace CQRSMagic.Events.Sourcing
{
    public interface IEventStore
    {
        Task<TAggregate> GetAggregateAsync<TAggregate>(Guid aggregateId) where TAggregate : IAggregate;
        Task SaveEventsAsync(IEnumerable<IEvent> events);
    }
}