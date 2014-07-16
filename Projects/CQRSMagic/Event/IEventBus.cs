using System.Collections.Generic;
using System.Threading.Tasks;

namespace CQRSMagic.Event
{
    public interface IEventBus
    {
        Task SendEventsAsync(IEnumerable<IEvent> events);
    }
}