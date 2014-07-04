using System.Collections.Generic;
using System.Threading.Tasks;

namespace CQRSMagic.Events.Publishing
{
    public interface IEventPublisher
    {
        Task PublishEventsAsync(IEnumerable<IEvent> events);
    }
}