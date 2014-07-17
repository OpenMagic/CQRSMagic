using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace CQRSMagic.Event
{
    public interface IEventBus
    {
        Task SendEventsAsync(IEnumerable<IEvent> events);
        void RegisterHandlers(Assembly searchAssembly);
    }
}