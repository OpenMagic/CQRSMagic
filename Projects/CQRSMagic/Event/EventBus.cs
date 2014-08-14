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

        public void RegisterHandlers(Assembly searchAssembly)
        {
            EventHandlers.RegisterHandlers(searchAssembly);
        }

        private async Task SendEventsAsync(ICollection<IEvent> events)
        {
            LogTo.Trace("Sending {0:N0} events.", events.Count);

            await EventStore.SaveEventsAsync(events);
            await Task.WhenAll(events.Select(SendEventAsync));

            LogTo.Trace("Sent {0:N0} events.", events.Count);
        }

        public void RegisterHandler<TEvent>(Func<TEvent, Task> handler) where TEvent : IEvent
        {
            EventHandlers.RegisterHandler(handler);
        }

        private async Task SendEventAsync(IEvent @event)
        {
            LogTo.Trace("Sending {0} event.", @event.GetType());

            var eventHandlers = EventHandlers.GetEventHandlers(@event).ToArray();

            LogTo.Trace("Sending {0} event to {1} event handlers.", @event.GetType(), eventHandlers.Length);

            await Task.WhenAll(eventHandlers.Select(eventHandler => SendEventToEventHandler(@event, eventHandler)));

            LogTo.Trace("Sent {0} event to {1} event handlers.", @event.GetType(), eventHandlers.Length);
            LogTo.Trace("Sent {0} event.", @event.GetType());
        }

        private static async Task SendEventToEventHandler(IEvent @event, Func<IEvent, Task> eventHandler)
        {
            LogTo.Trace("Sending {0} event to event handler.", @event.GetType());

            await eventHandler(@event);

            LogTo.Trace("Sent {0} event to event handler.", @event.GetType());
        }
    }
}