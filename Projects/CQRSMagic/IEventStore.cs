using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.CQRS
{
    public interface IEventStore
    {
        Task<TAggregate> GetAggregate<TAggregate>(Guid aggregateId) where TAggregate : IAggregate, new();

        Task<IEnumerable<IEvent>> GetEventsFor<TAggregate>(Guid aggregateId) where TAggregate : IAggregate;

        Task SaveEventsFor(Type aggregateType, Guid aggregateId, IEnumerable<IEvent> events);
    }
}