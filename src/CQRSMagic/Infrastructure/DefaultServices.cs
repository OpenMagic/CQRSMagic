using System;
using System.Collections.Generic;

namespace CQRSMagic.Infrastructure
{
    internal static class DefaultServices
    {
        public static bool TryGetInstance(Type serviceType, out object service)
        {
            if (Instances.TryGetValue(serviceType, out service))
            {
                return true;
            }

            Func<object> factory;

            if (!Factories.TryGetValue(serviceType, out factory))
            {
                return false;
            }

            service = factory();
            return true;
        }

        private static Dictionary<Type, object> GetDefaultInstances()
        {
            return new Dictionary<Type, object>();
        }

        private static Dictionary<Type, Func<object>> GetDefaultFactories()
        {
            var dictionary = new Dictionary<Type, Func<object>>
            {
                {typeof (ISubscriptionFinder), () => new SubscriptionFinder()},
                {
                    typeof (IAssemblyEventSubscriber), () => new AssemblyEventSubscriber(DependencyFactory.GetInstance<ISubscriptionFinder>())
                }
            };

            return dictionary;
        }

        private static readonly Dictionary<Type, object> Instances = GetDefaultInstances();
        private static readonly Dictionary<Type, Func<object>> Factories = GetDefaultFactories();
    }
}