using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CQRSMagic.Event;

namespace CQRSMagic.Domain
{
    internal class AggregateEventHandlers
    {
        private readonly ConcurrentDictionary<Type, Dictionary<Type, MethodInfo>> Handlers;

        internal AggregateEventHandlers()
        {
            Handlers = new ConcurrentDictionary<Type, Dictionary<Type, MethodInfo>>();
        }

        internal MethodInfo FindEventHandler(Type aggregateType, IEvent @event)
        {
            MethodInfo method;

            var aggregateEventHandlers = Handlers.GetOrAdd(aggregateType, FindEventHandlers(aggregateType));

            return aggregateEventHandlers.TryGetValue(@event.GetType(), out method) ? method : null;
        }

        internal Dictionary<Type, MethodInfo> FindEventHandlers(Type type)
        {
            // todo: what if IApplyEvent adds new methods
            var handlers =
                from @interface in type.GetInterfaces()
                where @interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(IApplyEvent<>)
                select new { EventType = @interface.GetGenericArguments()[0], MethodInfo = @interface.GetMethods().Single() };

            return handlers.ToDictionary(x => x.EventType, x => x.MethodInfo);
        }

    }
}