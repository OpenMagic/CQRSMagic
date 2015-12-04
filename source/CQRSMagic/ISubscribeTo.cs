namespace CQRSMagic
{
    public interface ISubscribeTo<in TEvent> where TEvent : IEvent
    {
        void SubscriptionHandler(TEvent e);
    }
}