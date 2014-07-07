using System.Threading.Tasks;
using CQRSMagic.Events.Messaging;

namespace CQRSMagic.Events.Publishing
{
    public interface ISubscribeTo<in TEvent> where TEvent : IEvent
    {
        Task HandleEventAsync(TEvent @event);
    }
}