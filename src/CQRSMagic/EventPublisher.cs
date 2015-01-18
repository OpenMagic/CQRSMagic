using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace CQRSMagic
{
    public class EventPublisher : IEventPublisher
    {
        private readonly ConcurrentDictionary<Type, IList<Action<IEvent>>> _eventSubscribers = new ConcurrentDictionary<Type, IList<Action<IEvent>>>();

        public void PublishEvents(IEvent[] events)
        {
            foreach (var @event in events)
            {
                IList<Action<IEvent>> subscribers;

                if (!_eventSubscribers.TryGetValue(@event.GetType(), out subscribers))
                {
                    continue;
                }

                foreach (var subscriber in subscribers)
                {
                    subscriber(@event);
                }
            }
        }

        public void SubscribeTo<TEvent>(Action<TEvent> action) where TEvent : class, IEvent
        {
            var subscribers = _eventSubscribers.GetOrAdd(typeof (TEvent), key => new List<Action<IEvent>>());
            
            subscribers.Add(e => action((TEvent) e));
        }
    }
}