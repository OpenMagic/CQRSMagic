using System.Collections.Generic;
using System.Threading.Tasks;
using CQRSMagic.Events.Messaging;

namespace CQRSMagic.Events.Publishing
{
    public interface IEventPublisher
    {
        Task PublishEventsAsync(IEnumerable<IEvent> events);
    }
}