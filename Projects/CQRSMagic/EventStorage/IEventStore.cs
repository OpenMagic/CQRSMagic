using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CQRSMagic.Event;

namespace CQRSMagic.EventStorage
{
    public interface IEventStore
    {
        Task<IEnumerable<IEvent>> FindEventsAsync(Guid aggregateId);
        Task<TAggregate> GetAggregateAsync<TAggregate>(Guid aggregateId);
    }
}