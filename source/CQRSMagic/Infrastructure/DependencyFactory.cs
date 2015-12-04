using System;
using Microsoft.Practices.ServiceLocation;

namespace CQRSMagic.Infrastructure
{
    // DependencyFactory provides support for CommonServiceLocator and DefaultServices. 
    // When CommonServiceLocator fails to provide a service then it calls on DefaultServices to provide it.
    internal static class DependencyFactory
    {
        internal static TService GetInstance<TService>()
        {
            return (TService) GetInstance(typeof (TService));
        }

        internal static object GetInstance(Type instanceType)
        {
            object instance;

            if (TryGetInstance(instanceType, out instance))
            {
                return instance;
            }

            if (DefaultServices.TryGetInstance(instanceType, out instance))
            {
                return instance;
            }

            try
            {
                return Activator.CreateInstance(instanceType);
            }
            catch (Exception exception)
            {
                var message = string.Format(
                    "Cannot create instance of {0}.\r\n" +
                    "Configure CommonServiceLocator (https://commonservicelocator.codeplex.com/) to provide an instance.",
                    instanceType);

                throw new InvalidOperationException(message, exception);
            }
        }

        private static bool TryGetInstance(Type instanceType, out object instance)
        {
            if (ServiceLocator.IsLocationProviderSet && ServiceLocator.Current.TryGetInstance(instanceType, out instance))
            {
                return true;
            }

            instance = null;
            return false;
        }

    }
}