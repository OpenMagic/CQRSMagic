using System;
using AzureMagic.Tables;
using CQRSMagic.Support;
using Microsoft.Practices.ServiceLocation;

namespace CQRSMagic.Azure.Support
{
    internal static class IoC
    {
        static IoC()
        {
            Initialize();
        }

        internal static void Initialize()
        {
        }

        internal static TService Get<TService>()
        {
            try
            {
                return ServiceLocator.Current.Get<TService>();
            }
            catch (Exception)
            {
                var service = FindService<TService>();

                // ReSharper disable once CompareNonConstrainedGenericWithNull
                if (service == null)
                {
                    throw;
                }

                return service;
            }
        }

        private static TService FindService<TService>()
        {
            return (TService) FindService(typeof(TService));
        }

        private static object FindService(Type serviceType)
        {
            if (serviceType == typeof(IAzureEventSerializer))
            {
                return new AzureEventSerializer();
            }

            if (serviceType == typeof(ISettings))
            {
                return Settings.Current;
            }

            if (serviceType == typeof(IAzureTableRepositoryLogger))
            {
                return new NullAzureTableRepositoryLogger();
            }

            return null;
        }
    }
}