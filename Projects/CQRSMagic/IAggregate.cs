using System.Collections.Generic;
using System.Threading.Tasks;

namespace CQRSMagic
{
    public interface IAggregate
    {
        Task SendEvents(IEnumerable<IEvent> events);
    }
}