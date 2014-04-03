using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.CQRS
{
    public interface IAggregate
    {
        Task SendEvents(IEnumerable<IEvent> events);
    }
}