using System;
using CQRSMagic.Command;
using CQRSMagic.Event;
using CQRSMagic.EventStorage;
using Microsoft.Practices.ServiceLocation;

namespace CQRSMagic.Support
{
    internal static class IoC
    {
        private static readonly Lazy<ICommandHandlers> CommandHandlers = new Lazy<ICommandHandlers>(CreateCommandHandlers);
        private static readonly Lazy<IEventHandlers> EventHandlers = new Lazy<IEventHandlers>(CreateEventHandlers);

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
            if (serviceType == typeof(IServiceLocator))
            {
                return ServiceLocator.Current;
            }

            if (serviceType == typeof(ICommandHandlers))
            {
                return CommandHandlers.Value;
            }

            if (serviceType == typeof(IEventBus))
            {
                return new EventBus();
            }

            if (serviceType == typeof(IEventHandlers))
            {
                return EventHandlers.Value;
            }

            if (serviceType == typeof(IEventStoreRepository))
            {
                return Activator.CreateInstance(Get<ISettings>().EventStoreRepositoryType);
            }

            if (serviceType == typeof(ISettings))
            {
                return Settings.Current;
            }

            return null;
        }

        private static ICommandHandlers CreateCommandHandlers()
        {
            var commandHandlers = new CommandHandlers(Get<IServiceLocator>());

            foreach (var domainAssembly in Get<ISettings>().DomainAssemblies)
            {
                commandHandlers.RegisterHandlers(domainAssembly);
            }

            return commandHandlers;
        }


        private static IEventHandlers CreateEventHandlers()
        {
            var eventHandlers = new EventHandlers(Get<IServiceLocator>());

            foreach (var domainAssembly in Get<ISettings>().DomainAssemblies)
            {
                eventHandlers.RegisterHandlers(domainAssembly);
            }

            return eventHandlers;
        }
    }
}