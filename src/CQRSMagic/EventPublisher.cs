using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using CQRSMagic.Infrastructure;

namespace CQRSMagic
{
    public class EventPublisher : IEventPublisher
    {
        private readonly IAssemblyEventSubscriber _assemblyEventSubscriber;
        private readonly ConcurrentDictionary<Type, IList<Action<IEvent>>> _eventSubscribers = new ConcurrentDictionary<Type, IList<Action<IEvent>>>();

        public EventPublisher()
            : this(DependencyFactory.GetInstance<IAssemblyEventSubscriber>())
        {
        }

        public EventPublisher(IAssemblyEventSubscriber assemblyEventSubscriber)
        {
            _assemblyEventSubscriber = assemblyEventSubscriber;
        }

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
            var subscribers = _eventSubscribers.GetOrAdd(typeof(TEvent), key => new List<Action<IEvent>>());

            subscribers.Add(e => action((TEvent)e));
        }

        public void SubscribeEventHandlersIn(Assembly assembly)
        {
            _assemblyEventSubscriber.SubscribeEventHandlers(assembly, this);
        }
    }
}
