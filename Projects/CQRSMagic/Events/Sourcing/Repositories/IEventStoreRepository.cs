using System;
using System.Collections.Generic;
using CQRSMagic.Domain;
using CQRSMagic.Events.Messaging;

namespace CQRSMagic.Events.Sourcing.Repositories
{
    public interface IEventStoreRepository
    {
        IEnumerable<IEvent> GetEvents<TAggregate>(Guid aggregateId) where TAggregate : IAggregate;
        void SaveEvents(IEnumerable<IEvent> events);
    }
}