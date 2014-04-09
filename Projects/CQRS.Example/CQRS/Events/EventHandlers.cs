using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Example.CQRS.Common;
using Microsoft.Practices.ServiceLocation;

namespace CQRS.Example.CQRS.Events
{
    public class EventHandlers : MessageHandlersBase<IEvent, Task>, IEventHandlers
    {
        private readonly List<KeyValuePair<Type, Func<IEvent, Task>>> Handlers;

        public EventHandlers(IServiceLocator container)
            : base(container)
        {
            Handlers = new List<KeyValuePair<Type, Func<IEvent, Task>>>();
        }

        public IEnumerable<Func<IEvent, Task>> GetHandlers(Type eventType)
        {
            return
                from keyValuePair in Handlers
                where keyValuePair.Key == eventType
                select keyValuePair.Value;
        }

        protected override void AddHandler(Type eventType, Func<IEvent, Task> eventHandlerFunc)
        {
            Handlers.Add(new KeyValuePair<Type, Func<IEvent, Task>>(eventType, eventHandlerFunc));
        }

        protected override Type GetMessageHandlerType()
        {
            return typeof(IEventHandler<>);
        }

        protected override Type GetMessageHandlerType<TMessage>()
        {
            return typeof(IEventHandler<TMessage>);
        }
    }
}