using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQRSMagic.Event
{
    public class EventBus : IEventBus
    {
        private readonly ConcurrentDictionary<Type, List<Func<IEvent, Task>>> Handlers;

        public EventBus()
        {
            Handlers = new ConcurrentDictionary<Type, List<Func<IEvent, Task>>>();
        }

        public Task SendEventsAsync(IEnumerable<IEvent> events)
        {
            var tasks = new List<Task>();

            foreach (var @event in events)
            {
                tasks.AddRange(SendEventAsync(@event));
            }

            return Task.FromResult(tasks.AsEnumerable());
        }

        public void RegisterHandler<TEvent>(Func<TEvent, Task> handler) where TEvent : IEvent
        {
            var key = typeof(TEvent);
            var eventHandlers = Handlers.GetOrAdd(key, new List<Func<IEvent, Task>>());

            Func<IEvent, Task> value = @event => handler((TEvent) @event);

            eventHandlers.Add(value);
        }

        private IEnumerable<Task> SendEventAsync(IEvent @event)
        {
            List<Func<IEvent, Task>> eventHandlers;

            if (!Handlers.TryGetValue(@event.GetType(), out eventHandlers))
            {
                return Enumerable.Empty<Task>();
            }

            var tasks = eventHandlers.Select(eventHandler => eventHandler(@event));

            return tasks;
        }
    }
}