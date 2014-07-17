using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQRSMagic.Event
{
    public class EventHandlers : IEventHandlers
    {
        private readonly ConcurrentDictionary<Type, List<Func<IEvent, Task>>> Handlers;

        public EventHandlers()
        {
            Handlers = new ConcurrentDictionary<Type, List<Func<IEvent, Task>>>();
            
        }

        public void RegisterHandler<TEvent>(Func<TEvent, Task> handler)
        {
            var key = typeof(TEvent);
            var eventHandlers = Handlers.GetOrAdd(key, new List<Func<IEvent, Task>>());

            Func<IEvent, Task> value = @event => handler((TEvent)@event);

            eventHandlers.Add(value);
        }

        public IEnumerable<Func<IEvent, Task>> GetEventHandlers(IEvent @event)
        {
            List<Func<IEvent, Task>> eventHandlers;

            return Handlers.TryGetValue(@event.GetType(), out eventHandlers) ? eventHandlers : Enumerable.Empty<Func<IEvent, Task>>();
        }
    }
}
