using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;

namespace CQRS.Example.CQRS.Events
{
    public class EventHandlers : IEventHandlers
    {
        private readonly IServiceLocator Container;
        private readonly List<KeyValuePair<Type, Func<IEvent, Task>>> Handlers;

        public EventHandlers(IServiceLocator container)
        {
            Container = container;
            Handlers = new List<KeyValuePair<Type, Func<IEvent, Task>>>();
        }

        public IEnumerable<Func<IEvent, Task>> GetHandlers(Type eventType)
        {
            return
                from keyValuePair in Handlers
                where keyValuePair.Key == eventType
                select keyValuePair.Value;
        }

        public void RegisterHandler<TEventHandler, TEvent>() where TEvent : class, IEvent
        {
            RegisterHandler(typeof(TEventHandler), typeof(IEventHandler<TEvent>), typeof(TEvent));
        }

        private void RegisterHandler(Type eventHandlerClass, Type eventHandlerInterface, Type eventType)
        {
            var eventHandlerFunc = CreateEventHandlerFunc(eventHandlerClass, eventHandlerInterface);

            Handlers.Add(new KeyValuePair<Type, Func<IEvent, Task>>(eventType, eventHandlerFunc));
        }

        private Func<IEvent, Task> CreateEventHandlerFunc(Type eventHandlerClass, Type eventHandlerInterface)
        {
            // todo: assuming that IEventHandler<TEvent> has only 1 method.
            var handleMethod = eventHandlerInterface.GetMethods().Single();

            return @event =>
            {
                var obj = Container.GetInstance(eventHandlerClass);
                var result = handleMethod.Invoke(obj, new object[] {@event});
                var task = (Task) result;

                return task;
            };
        }
    }
}