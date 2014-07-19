using System;
using System.Collections.Generic;
using System.Reflection;

namespace CQRSMagic
{
    public class Settings : ISettings
    {
        protected Settings(IEnumerable<Assembly> domainAssemblies, Type eventStoreRepositoryType)
        {
            DomainAssemblies = domainAssemblies;
            EventStoreRepositoryType = eventStoreRepositoryType;
        }

        public static ISettings Current { get; private set; }

        public IEnumerable<Assembly> DomainAssemblies { get; private set; }
        public Type EventStoreRepositoryType { get; private set; }

        protected static void Initialize(IEnumerable<Assembly> domainAssemblies, Type eventStoreRepositoryType)
        {
            Initialize(new Settings(domainAssemblies, eventStoreRepositoryType));
        }

        protected static void Initialize(ISettings settings)
        {
            Current = settings;
        }
    }
}