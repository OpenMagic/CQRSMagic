using System;
using System.Collections.Generic;

namespace CQRSMagic
{
    public interface IEntity
    {
        Guid Id { get; }

        void ApplyEvents(IEnumerable<IEvent> events);
    }
}