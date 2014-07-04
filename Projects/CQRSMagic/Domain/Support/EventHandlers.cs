using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CQRSMagic.Events.Messaging;

namespace CQRSMagic.Domain.Support
{
    internal class EventHandlers
    {
        private readonly Lazy<Dictionary<Type, MethodInfo>> Handlers;

        internal EventHandlers(Type type)
        {
            // todo: unit tests
            Handlers = new Lazy<Dictionary<Type, MethodInfo>>(() => FindEventHandlers(type));
        }

        internal Dictionary<Type, MethodInfo> FindEventHandlers(Type type)
        {
            // todo: unit tests

            // todo: what if IApplyEvent adds new methods
            var handlers =
                from @interface in type.GetInterfaces()
                where @interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(IApplyEvent<>)
                select new {EventType = @interface.GetGenericArguments()[0], MethodInfo = @interface.GetMethods().Single()};

            return handlers.ToDictionary(x => x.EventType, x => x.MethodInfo);
        }

        internal MethodInfo FindEventHandler(IEvent @event)
        {
            // todo: unit tests
            MethodInfo method;

            return Handlers.Value.TryGetValue(@event.GetType(), out method) ? method : null;
        }
    }
}