using System;

namespace CQRSMagic
{
    public interface IEventPublisher
    {
        void PublishEvents(IEvent[] events);
        void SubscribeTo<TEvent>(Action<TEvent> action) where TEvent : class, IEvent;
    }
}