using System.Collections.Generic;
using System.Reflection;
using CQRSMagic.Azure.Support;

namespace CQRSMagic.Azure
{
    public class Settings : CQRSMagic.Settings, ISettings
    {
        private Settings(IEnumerable<Assembly> domainAssemblies, string connectionString, string eventsTableName)
            : base(domainAssemblies, typeof(AzureEventStoreRepository))
        {
            ConnectionString = connectionString;
            EventsTableName = eventsTableName;
        }

        public new static ISettings Current { get; private set; }

        public string ConnectionString { get; private set; }
        public string EventsTableName { get; private set; }

        public static void Initialize(IEnumerable<Assembly> domainAssemblies, string connectionString, string eventsTableName)
        {
            Initialize(new Settings(domainAssemblies, connectionString, eventsTableName));
        }

        public static void Initialize(ISettings settings)
        {
            Current = settings;
            IoC.Initialize();
            CQRSMagic.Settings.Initialize(settings);
        }
    }
}