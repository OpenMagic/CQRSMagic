using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CQRSMagic.Event;

namespace CQRSMagic.EventStorage
{
    public class EventStore : IEventStore
    {
        public Task<IEnumerable<IEvent>> FindEventsAsync(Guid aggregateId)
        {
            throw new NotImplementedException();
        }

        public Task<TAggregate> GetAggregateAsync<TAggregate>(Guid aggregateId)
        {
            throw new NotImplementedException();
        }
    }
}