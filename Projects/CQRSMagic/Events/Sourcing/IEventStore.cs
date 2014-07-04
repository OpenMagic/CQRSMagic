using System;
using System.Collections.Generic;
using CQRSMagic.Domain;
using CQRSMagic.Events.Messaging;

namespace CQRSMagic.Events.Sourcing
{
    public interface IEventStore
    {
        TAggregate GetAggregate<TAggregate>(Guid aggregateId) where TAggregate : IAggregate;
        void SaveEvents(IEnumerable<IEvent> events);
    }
}