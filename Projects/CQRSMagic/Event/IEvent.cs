using System;

namespace CQRSMagic.Event
{
    public interface IEvent
    {
        Guid AggregateId { get; }
        DateTimeOffset EventCreated { get; }
    }
}