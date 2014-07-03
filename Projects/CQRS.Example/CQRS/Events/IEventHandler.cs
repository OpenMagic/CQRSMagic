using System.Threading.Tasks;
using CQRS.Example.CQRS.Common;

namespace CQRS.Example.CQRS.Events
{
    public interface IEventHandler<in TEvent> : IMessageHandler<TEvent> where TEvent : IEvent
    {
        Task Handle(TEvent @event);
    }
}