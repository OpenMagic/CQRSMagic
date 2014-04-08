using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQRS.Example.CQRS.Events
{
    public class EventPublisher : IEventPublisher
    {
        private readonly IEventHandlers EventHandlers;

        public EventPublisher(IEventHandlers eventHandlers)
        {
            EventHandlers = eventHandlers;
        }

        public IEnumerable<Task> PublishEvents(IEnumerable<IEvent> events)
        {
            return
                from e in events
                from handler in EventHandlers.GetHandlers(e.GetType())
                select handler(e);
        }
    }
}