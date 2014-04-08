using System.Collections.Generic;
using System.Threading.Tasks;
using CQRS.Example.CQRS.Events;

namespace CQRS.Example.CQRS
{
    public interface IEventRepository
    {
        Task SaveEvents(IEnumerable<IEvent> events);
    }
}