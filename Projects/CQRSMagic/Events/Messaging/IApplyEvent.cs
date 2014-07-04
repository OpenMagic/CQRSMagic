namespace CQRSMagic.Events
{
    public interface IApplyEvent<in TEvent> where TEvent : IEvent
    {
        void ApplyEvent(TEvent @event);
    }
}