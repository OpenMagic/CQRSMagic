using System.Threading.Tasks;

namespace CQRSMagic.Events.Publishing
{
    public interface ISubscribeTo<in TEvent> where TEvent : IEvent
    {
        Task HandleEventAsync(TEvent @event);
    }
}