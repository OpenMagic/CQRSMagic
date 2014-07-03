using System;

namespace CQRSMagic
{
    public interface IEventStore
    {
        TAggregate GetAggregate<TAggregate>(Guid aggregateId) where TAggregate : IAggregate;
    }
}