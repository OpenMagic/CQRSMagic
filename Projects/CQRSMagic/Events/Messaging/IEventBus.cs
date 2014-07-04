using System.Collections.Generic;

namespace CQRSMagic.Events.Messaging
{
    public interface IEventBus
    {
        void SendEvents(IEnumerable<IEvent> events);
    }
}