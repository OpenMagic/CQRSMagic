using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CQRSMagic
{
    public interface IEventStoreRepository
    {
        Task<IEnumerable<IEvent>> FindEventsFor(Type aggregateType, Guid aggregateId);
        Task SaveEventsFor(Type aggregateType, Guid aggregateId, IEnumerable<IEvent> events);
    }
}