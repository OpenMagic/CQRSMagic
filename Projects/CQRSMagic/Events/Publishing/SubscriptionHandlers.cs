using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CQRSMagic.Events.Publishing.Support;

namespace CQRSMagic.Events.Publishing
{
    public class SubscriptionHandlers : ISubscriptionHandlers
    {
        private readonly Lazy<Dictionary<Type, IEnumerable<Func<IEvent, Task>>>> Handlers;

        public SubscriptionHandlers(IEnumerable<Assembly> eventSubscriberAssemblies)
        {
            // todo: unit tests
            Handlers = new Lazy<Dictionary<Type, IEnumerable<Func<IEvent, Task>>>>(() => FindHandlers(eventSubscriberAssemblies));
        }

        public IEnumerable<Func<IEvent, Task>> FindSubscriptionsFor(IEvent @event)
        {
            // todo: unit tests
            IEnumerable<Func<IEvent, Task>> handler;

            var handlers = Handlers.Value;

            return handlers.TryGetValue(@event.GetType(), out handler) ? handler : new List<Func<IEvent, Task>>();
        }

        internal static Dictionary<Type, IEnumerable<Func<IEvent, Task>>> FindHandlers(IEnumerable<Assembly> eventSubscriberAssemblies)
        {
            // todo: unit tests

            var allTypes =
                from assembly in eventSubscriberAssemblies
                from type in assembly.GetTypes()
                select type;

            var eventSubscribers =
                from type in allTypes
                from @interface in type.GetInterfaces()
                where @interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(ISubscribeTo<>)
                let eventType = @interface.GetGenericArguments()[0]
                select new SubscriptionHandler(type, @interface, eventType);

            var groupedEventSubscribers =
                from e in eventSubscribers
                group e by e.EventType
                into g
                select new {EventType = g.Key, EventHandlers = g.Select(x => x.HandleEventAsync)};


            return groupedEventSubscribers.ToDictionary(x => x.EventType, x => x.EventHandlers);
        }
    }
}