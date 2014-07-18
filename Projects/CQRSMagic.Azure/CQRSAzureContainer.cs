using System;
using System.Collections.Generic;
using System.Reflection;
using CQRSMagic.Command;
using CQRSMagic.Event;
using CQRSMagic.EventStorage;
using CQRSMagic.IoC;
using NullGuard;

namespace CQRSMagic.Azure
{
    public class CQRSAzureContainer : BasicIoC
    {
        public CQRSAzureContainer(string connectionString, string eventsTableName, IEnumerable<Assembly> domainAssemblies)
            : this(connectionString, eventsTableName, domainAssemblies, null)
        {
        }

        public CQRSAzureContainer(string connectionString, string eventsTableName, IEnumerable<Assembly> domainAssemblies, [AllowNull] Func<Type, object> unboundTypeFactory)
            : base(unboundTypeFactory)
        {
            Bind<IEventBus>(new EventBus(this));
            Bind<IAzureEventSerializer>(container => new AzureEventSerializer());
            Bind<IEventStoreRepository>(container => new AzureEventStoreRepository(connectionString, eventsTableName, container.Get<IAzureEventSerializer>()));
            Bind<IEventStore>(container => new EventStore(container.Get<IEventStoreRepository>(), container));
            Bind<ICommandBus>(new CommandBus(Get<IEventStore>(), Get<IEventBus>(), this));

            var commandBus = Get<ICommandBus>();
            var eventBus = Get<IEventBus>();

            foreach (var domainAssembly in domainAssemblies)
            {
                commandBus.RegisterHandlers(domainAssembly);
                eventBus.RegisterHandlers(domainAssembly);
            }
        }
    }
}