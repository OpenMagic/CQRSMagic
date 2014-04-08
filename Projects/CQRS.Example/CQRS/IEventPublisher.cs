using System.Collections.Generic;
using System.Threading.Tasks;
using CQRS.Example.CQRS.Events;

namespace CQRS.Example.CQRS
{
    public interface IEventPublisher
    {
        IEnumerable<Task> PublishEvents(IEnumerable<IEvent> events);
        void RegisterHandler<TEventHandler, TEvent>() where TEvent : class, IEvent;
    }
}