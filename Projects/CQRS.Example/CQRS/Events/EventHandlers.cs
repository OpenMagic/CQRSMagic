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

        public Task RegisterHandlers(IEnumerable<Type> types)
        {
            return Task.Factory.StartNew(() => RegisterHandlersSync(types));
        }

        private void RegisterHandlersSync(IEnumerable<Type> types)
        {
            var eventHandlers =
                from type in types
                where type.IsClass && !type.IsAbstract
                from @interface in type.GetInterfaces()
                where @interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(IEventHandler<>)
                select new { EventHandlerClass = type, EventHandlerInterface = @interface, EventType = @interface.GetGenericArguments().Single() };

            // 9 Apr 2014, Visual Studio 2013. Tempting to use Parallel.ForEach but it seemed to be the cause of NullReferenceException being thrown by RegisterHandler.
            foreach (var eventHandler in eventHandlers)
            {
                RegisterHandler(eventHandler.EventHandlerClass, eventHandler.EventHandlerInterface, eventHandler.EventType);
            }
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