using System;
using System.Collections.Generic;
using CQRSMagic.Domain;

namespace CQRSMagic.Events
{
    public interface IEventStore
    {
        TAggregate GetAggregate<TAggregate>(Guid aggregateId) where TAggregate : IAggregate;
        void SaveEvents(IEnumerable<IEvent> events);
    }
}