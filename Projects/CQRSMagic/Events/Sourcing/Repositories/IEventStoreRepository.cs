using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CQRSMagic.Domain;
using CQRSMagic.Events.Messaging;

namespace CQRSMagic.Events.Sourcing.Repositories
{
    public interface IEventStoreRepository
    {
        Task<IEnumerable<IEvent>> GetEventsAsync<TAggregate>(Guid aggregateId) where TAggregate : IAggregate;
        Task SaveEventsAsync(IEnumerable<IEvent> events);
    }
}