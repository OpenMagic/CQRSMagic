using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CQRSMagic.Event;

namespace CQRSMagic.EventStorage
{
    public interface IEventStoreRepository
    {
        Task<IEnumerable<IEvent>> FindAllEventsAsync();
        Task<IEnumerable<IEvent>> FindEventsAsync(Guid aggregateId);

        Task SaveEventsAsync(IEnumerable<IEvent> events);
    }
}