using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Anotar.CommonLogging;
using CQRSMagic.Events.Messaging;
using Microsoft.Practices.ServiceLocation;

namespace CQRSMagic.Events.Publishing.Support
{
    internal class SubscriptionHandler
    {
        private readonly Type ConcreteType;
        public readonly Type EventType;
        public readonly Func<IEvent, Task> HandleEventAsync;
        private readonly MethodInfo HandleMethod;

        public SubscriptionHandler(Type concreteType, Type subscribeToType, Type eventType)
        {
            ConcreteType = concreteType;
            EventType = eventType;
            HandleEventAsync = ExecuteHandleEventAsync;

            // todo: what if ISubscribeTo adds new methods
            HandleMethod = subscribeToType.GetMethods().Single();
        }

        private Task ExecuteHandleEventAsync(IEvent @event)
        {
            try
            {
                var obj = ServiceLocator.Current.GetInstance(ConcreteType);

                var result = HandleMethod.Invoke(obj, new object[] {@event});
                var task = (Task) result;

                return task;
            }
            catch (Exception exception)
            {
                LogTo.ErrorException(exception.Message, exception);
                throw;
            }
        }
    }
}