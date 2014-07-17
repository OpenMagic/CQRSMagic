using System.Threading.Tasks;

namespace CQRSMagic.Event
{
    public interface IApplyEvent<in TEvent> where TEvent : IEvent
    {
        Task ApplyEventAsync(TEvent @event);
    }
}