﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CQRSMagic.Support;

namespace CQRSMagic.Event
{
    public class EventBus : IEventBus
    {
        private readonly IEventHandlers EventHandlers;

        public EventBus()
            : this(IoC.Get<IEventHandlers>())
        {
        }

        public EventBus(IEventHandlers eventHandlers)
        {
            EventHandlers = eventHandlers;
        }

        public Task SendEventsAsync(IEnumerable<IEvent> events)
        {
            var tasks = new List<Task>();

            foreach (var @event in events)
            {
                tasks.AddRange(SendEventAsync(@event));
            }

            return Task.WhenAll(tasks);
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