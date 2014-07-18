using System.Collections.Generic;
using System.Reflection;
using CQRSMagic.Command;
using CQRSMagic.Event;
using CQRSMagic.EventStorage;
using CQRSMagic.IoC;

namespace CQRSMagic.Azure
{
    public static class DependencyContainerExtensions
    {
        private static EventBus EventBus;
        private static CommandBus CommandBus;

        public static void AddAzureBindings(this IDependencyContainer container, string connectionString, string eventsTableName, IEnumerable<Assembly> domainAssemblies)
        {
            container.Bind<ICommandBus>(c => CommandBus);
            container.Bind<IEventBus>(c => EventBus);
            container.Bind<IAzureEventSerializer>(c => new AzureEventSerializer());
            container.Bind<IEventStoreRepository>(c => new AzureEventStoreRepository(connectionString, eventsTableName, c.Get<IAzureEventSerializer>()));
            container.Bind<IEventStore>(c => new EventStore(c.Get<IEventStoreRepository>(), c));

            EventBus = new EventBus(container);
            CommandBus = new CommandBus(container.Get<IEventStore>(), container.Get<IEventBus>(), container);

            foreach (var domainAssembly in domainAssemblies)
            {
                CommandBus.RegisterHandlers(domainAssembly);
                EventBus.RegisterHandlers(domainAssembly);
            }
        }
    }
}