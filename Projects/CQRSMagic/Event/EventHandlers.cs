﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Anotar.CommonLogging;
using CQRSMagic.IoC;
using OpenMagic;

namespace CQRSMagic.Event
{
    public class EventHandlers : IEventHandlers
    {
        private readonly IDependencyResolver DependencyResolver;
        private readonly ConcurrentDictionary<Type, List<Func<IEvent, Task>>> Handlers;

        public EventHandlers(IDependencyResolver dependencyResolver)
        {
            DependencyResolver = dependencyResolver;
            Handlers = new ConcurrentDictionary<Type, List<Func<IEvent, Task>>>();
        }

        public void RegisterHandler<TEvent>(Func<TEvent, Task> handler)
        {
            var key = typeof(TEvent);
            Func<IEvent, Task> value = @event => handler((TEvent) @event);

            RegisterHandler(key, value);
        }

        public IEnumerable<Func<IEvent, Task>> GetEventHandlers(IEvent @event)
        {
            List<Func<IEvent, Task>> eventHandlers;

            return Handlers.TryGetValue(@event.GetType(), out eventHandlers) ? eventHandlers : Enumerable.Empty<Func<IEvent, Task>>();
        }

        public IEnumerable<KeyValuePair<Type, IEnumerable<Func<IEvent, Task>>>> RegisterHandlers(Assembly searchAssembly)
        {
            LogTo.Trace("RegisterHandlers(searchAssembly: {0})", searchAssembly.GetName());

            var eventSubscribers =
                from type in searchAssembly.GetTypes()
                from @interface in type.GetInterfaces()
                where @interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(ISubscribeTo<>)
                let eventType = @interface.GetGenericArguments()[0]
                select new {eventType, handler = CreateEventHandler(type, @interface.GetMethods().Single())};

            foreach (var item in eventSubscribers)
            {
                RegisterHandler(item.eventType, item.handler);
            }

            var handlers = Handlers.Select(handler => new KeyValuePair<Type, IEnumerable<Func<IEvent, Task>>>(handler.Key, handler.Value));

            return handlers;
        }

        private Func<IEvent, Task> CreateEventHandler(Type eventHandlerType, MethodInfo eventHandlerMethod)
        {
            Func<IEvent, Task> handler = @event => HandleEvent(@event, eventHandlerType, eventHandlerMethod);

            return handler;
        }

        private Task HandleEvent(IEvent @event, Type eventHandlerType, MethodInfo eventHandlerMethod)
        {
            var eventHandler = DependencyResolver.Get(eventHandlerType);
            var result = eventHandlerMethod.Invoke(eventHandler, new object[] {@event});
            var task = (Task) result;

            return task;
        }

        private void RegisterHandler(Type eventType, Func<IEvent, Task> handler)
        {
            LogTo.Trace("RegisterHandler(eventType: {0}, handler)", eventType);

            var eventHandlers = Handlers.GetOrAdd(eventType, new List<Func<IEvent, Task>>());

            eventHandlers.Add(handler);
        }
    }
}