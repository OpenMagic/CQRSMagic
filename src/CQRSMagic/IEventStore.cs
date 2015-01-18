using System;

namespace CQRSMagic
{
    public interface IEventStore
    {
        void SaveEvents(Guid id, int version, IEvent[] events);
    }
}