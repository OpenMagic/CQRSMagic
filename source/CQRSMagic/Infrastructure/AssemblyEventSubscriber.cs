using System;
using System.Reflection;

namespace CQRSMagic.Infrastructure
{
    public class AssemblyEventSubscriber : IAssemblyEventSubscriber
    {
        private readonly ISubscriptionFinder _subscriptionFinder;

        public AssemblyEventSubscriber(ISubscriptionFinder subscriptionFinder)
        {
            _subscriptionFinder = subscriptionFinder;
        }

        public void SubscribeEventHandlers(Assembly inAssembly, IEventPublisher toEventPublisher)
        {
            var subscribers = _subscriptionFinder.FindSubscribers(inAssembly);

            foreach (var subscriber in subscribers)
            {
                var instanceType = subscriber.TypeInfo.DeclaringType;
                Func<object> getInstance = () => DependencyFactory.GetInstance(instanceType);
            }
        }
    }
}
