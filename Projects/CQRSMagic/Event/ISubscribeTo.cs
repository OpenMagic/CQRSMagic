using System.Threading.Tasks;

namespace CQRSMagic.Event
{
    public interface ISubscribeTo<in TEvent> where TEvent : IEvent
    {
        Task HandleEventAsync(TEvent @event);
    }
}