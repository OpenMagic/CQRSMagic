using System;

namespace CQRSMagic.Events
{
    public interface IEvent
    {
        Type AggregateType { get; }
        Guid AggregateId { get; }
    }
}