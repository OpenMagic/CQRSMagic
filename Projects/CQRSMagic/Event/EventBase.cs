using System;

namespace CQRSMagic.Event
{
    public abstract class EventBase : IEvent
    {
        public Guid AggregateId { get; protected set; }
    }
}