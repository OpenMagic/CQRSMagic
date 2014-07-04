using System;

namespace CQRSMagic.Events.Messaging
{
    public interface IEvent
    {
        Type AggregateType { get; }
        Guid AggregateId { get; }
    }
}