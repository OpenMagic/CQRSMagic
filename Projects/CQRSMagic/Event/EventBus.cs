using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CQRSMagic.EventStorage;
using CQRSMagic.Support;

namespace CQRSMagic.Event
{
    public class EventBus : IEventBus
    {
        private readonly IEventHandlers EventHandlers;
        private readonly IEventStore EventStore;

        public EventBus()
            : this(IoC.Get<IEventHandlers>(), IoC.Get<IEventStore>())
        {
        }

        public EventBus(IEventHandlers eventHandlers, IEventStore eventStore)
        {
            EventHandlers = eventHandlers;
            EventStore = eventStore;
        }

        public async Task SendEventsAsync(IEnumerable<IEvent> events)
        {
            await SendEventsAsync(events.ToArray());
        }

        private async Task SendEventsAsync(IEvent[] events)
        {
            await EventStore.SaveEventsAsync(events);

            var tasks = new List<Task>();

            foreach (var @event in events)
            {
                tasks.AddRange(SendEventAsync(@event));
            }

            await Task.WhenAll(tasks);
        }

        public void RegisterHandlers(Assembly searchAssembly)
        {
            EventHandlers.RegisterHandlers(searchAssembly);
        }

        public void RegisterHandler<TEvent>(Func<TEvent, Task> handler) where TEvent : IEvent
        {
            EventHandlers.RegisterHandler(handler);
        }

        private IEnumerable<Task> SendEventAsync(IEvent @event)
        {
            var eventHandlers = EventHandlers.GetEventHandlers(@event);
            var tasks = eventHandlers.Select(eventHandler => eventHandler(@event));

            return tasks;
        }
    }
}