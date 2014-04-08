using System.Collections.Generic;
using System.Threading.Tasks;

namespace CQRS.Example.CQRS.Events
{
    public interface IEventPublisher
    {
        IEnumerable<Task> PublishEvents(IEnumerable<IEvent> events);
        void RegisterHandler<TEventHandler, TEvent>() where TEvent : class, IEvent;
    }
}