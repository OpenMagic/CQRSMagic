using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Anotar.CommonLogging;
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
            LogTo.Trace("Sending {0:N0} events.", events.Length);

            await EventStore.SaveEventsAsync(events);

            var tasks = new List<Task>();

            foreach (var @event in events)
            {
                tasks.AddRange(SendEventAsync(@event));
            }

            await Task
                .WhenAll(tasks)
                .ContinueWith(c => LogTo.Trace("Sent {0:N0} events.", events.Length));
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
            LogTo.Trace("Sending {0} event.", @event.GetType());

            var eventHandlers = EventHandlers.GetEventHandlers(@event);
            var tasks = eventHandlers.Select(eventHandler =>
            {
                LogTo.Trace("Sending {0} event to event handler.", @event.GetType());

                return eventHandler(@event)
                    .ContinueWith(c => LogTo.Trace("Sent {0} event to event handler.", @event.GetType()));
            });

            LogTo.Trace("Sent {0} event.", @event.GetType());

            return tasks;
        }
    }
}