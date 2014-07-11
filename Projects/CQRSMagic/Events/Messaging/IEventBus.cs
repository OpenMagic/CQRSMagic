using System.Collections.Generic;
using System.Threading.Tasks;

namespace CQRSMagic.Events.Messaging
{
    public interface IEventBus
    {
        Task SendEventsAsync(IEnumerable<IEvent> events);
    }
}