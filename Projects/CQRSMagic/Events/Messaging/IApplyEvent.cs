namespace CQRSMagic.Events.Messaging
{
    public interface IApplyEvent<in TEvent> where TEvent : IEvent
    {
        void ApplyEvent(TEvent @event);
    }
}