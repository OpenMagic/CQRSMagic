using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Anotar.CommonLogging;
using CQRSMagic.Support;
using Microsoft.Practices.ServiceLocation;
using OpenMagic.Extensions.Collections.Generic;

namespace CQRSMagic
{
    public class SubscribeToHandlers : ISubscribeToHandlers
    {
        private static Lazy<Dictionary<Type, List<Action<Guid, IEvent>>>> Cache;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SubscribeToHandlers" /> class.
        /// </summary>
        /// <param name="container">Container to initialize types that implement IEventHandler&lt;&gt;</param>
        /// <param name="eventHandlerAssemblies">The assemblies that contain types that implement of IEventHandler&lt;&gt;</param>
        public SubscribeToHandlers(IServiceLocator container, IEnumerable<Assembly> eventHandlerAssemblies)
        {
            Cache = new Lazy<Dictionary<Type, List<Action<Guid, IEvent>>>>(() => GetSubscribeToHandlers(container, eventHandlerAssemblies));
        }

        public Task PublishEvents(Guid aggregateId, IEnumerable<IEvent> events)
        {
            // todo: is this the right way to do Task?
            return Task.Factory.StartNew(() => events.ForEach(e => PublishEvent(aggregateId, e)));
        }

        private static Task PublishEvent(Guid aggregateId, IEvent e)
        {
            // todo: is this the right way to do Task?
            return Task.Factory.StartNew(() =>
            {
                List<Action<Guid, IEvent>> handlers;

                if (!Cache.Value.TryGetValue(e.GetType(), out handlers))
                {
                    return;
                }

                // todo: is this the right way to do Task?
                handlers.ForEach(handler => Task.Factory.StartNew(() => handler(aggregateId, e)));
            });
        }

        private static Dictionary<Type, List<Action<Guid, IEvent>>> GetSubscribeToHandlers(IServiceLocator container, IEnumerable<Assembly> eventHandlerAssemblies)
        {
            var sw = Stopwatch.StartNew();

            ValidateISubscribeToHasOneMethod();

            var types =
                from assembly in eventHandlerAssemblies
                from type in assembly.GetTypes()
                select type;

            // Find and create an instance of all types that implement ISubscribeTo<>.
            var subscribeToObjects = (
                from type in types
                where type.Implements(typeof(ISubscribeTo<>))
                select new {Type = type, Obj = container.GetInstance(type)}).ToArray();

            // Get the methods that implement ISubscribeTo<>.Handle(IEvent e)
            // todo: magic string for event parameter name.
            var eventTypes =
                from subscribeToObject in subscribeToObjects
                from @interface in subscribeToObject.Type.GetInterfaces()
                where @interface.Name == typeof(ISubscribeTo<>).Name
                let m = @interface.GetMethods().Single()
                let e = new {EventType = m.GetParameters().Single(p => p.Name == "e").ParameterType, Action = CreateAction(subscribeToObject.Obj, m)}
                group e by e.EventType
                into g
                select new {EventType = g.Key, Actions = g.Select(x => x.Action).ToList()};

            var dictionary = eventTypes.ToDictionary(e => e.EventType, e => e.Actions);

            LogPerformance(sw);

            return dictionary;
        }

        private static Action<Guid, IEvent> CreateAction(object obj, MethodInfo method)
        {
            return (aggregateId, e) => method.Invoke(obj, new object[] {aggregateId, e});
        }

        private static void LogPerformance(Stopwatch sw)
        {
            if (sw.ElapsedMilliseconds < 10)
            {
                LogTo.Info("GetSubscribeToHandlers() took {0:N0}ms.", sw.ElapsedMilliseconds);
            }
            else
            {
                LogTo.Warn("GetSubscribeToHandlers() took {0:N0}ms.", sw.ElapsedMilliseconds);
            }
        }

        private static void ValidateISubscribeToHasOneMethod()
        {
            if (typeof(ISubscribeTo<>).GetMethods().Length > 1)
            {
                throw new Exception("Queries in GetSubscribeToHandlers() expect ISubscribeTo<> to have only 1 method. The queries will need to be modified.");
            }
        }
    }
}