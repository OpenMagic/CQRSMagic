using System.Collections.Generic;
using System.Threading.Tasks;
using CQRS.Example.CQRS.Events;

namespace CQRS.Example.CQRS
{
    public interface IEventStore
    {
        Task SaveEvents(IEnumerable<IEvent> events);
    }
}