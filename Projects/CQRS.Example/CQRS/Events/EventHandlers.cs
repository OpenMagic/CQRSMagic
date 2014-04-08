using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            Handlers.Add(new KeyValuePair<Type, Func<IEvent, Task>>(typeof(TEvent), Handler<TEventHandler, TEvent>()));
        }

        private Func<IEvent, Task> Handler<TEventHandler, TEvent>() where TEvent : class, IEvent
        {
            var handleMethod = GetHandleMethod<TEvent>();

            return command =>
            {
                var obj = Container.GetInstance<TEventHandler>();
                var task = handleMethod.Invoke(obj, new object[] { command });

                return (Task)task;
            };
        }

        private static MethodInfo GetHandleMethod<TEvent>() where TEvent : class, IEvent
        {
            var handler = typeof(IEventHandler<TEvent>);

            // assuming that IHandle<TEvent> has only 1 method.
            return handler.GetMethods().Single();
        }
    }
}