using System;
using System.Collections.Generic;
using CQRSMagic.Events.Messaging;

namespace CQRSMagic.Domain
{
    public interface IAggregate
    {
        Guid Id { get; }

        void ApplyEvents(IEnumerable<IEvent> events);
    }
}