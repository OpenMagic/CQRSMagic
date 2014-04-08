using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CQRS.Example.CQRS.Events;
using Microsoft.Practices.ServiceLocation;

namespace CQRS.Example.CQRS
{
    public class EventPublisher : IEventPublisher
    {
        private readonly IServiceLocator Container;
        private readonly List<KeyValuePair<Type, Func<IEvent, Task>>> Handlers = new List<KeyValuePair<Type, Func<IEvent, Task>>>();

        public EventPublisher(IServiceLocator container)
        {
            Container = container;
        }

        public IEnumerable<Task> PublishEvents(IEnumerable<IEvent> events)
        {
            return
                from e in events
                from handler in Handlers
                where handler.Key == e.GetType()
                select handler.Value(e);
        }

        public void RegisterHandler<TEventHandler, TEvent>() where TEvent : class, IEvent
        {
            Handlers.Add(new KeyValuePair<Type,Func<IEvent,Task>>(typeof(TEvent), Handler<TEventHandler, TEvent>()));
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