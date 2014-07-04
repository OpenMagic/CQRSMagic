using System.Collections.Generic;

namespace CQRSMagic.Events
{
    public interface IEventBus
    {
        void SendEvents(IEnumerable<IEvent> events);
    }
}