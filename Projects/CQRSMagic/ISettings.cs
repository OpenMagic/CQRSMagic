using System;
using System.Collections.Generic;
using System.Reflection;

namespace CQRSMagic
{
    public interface ISettings
    {
        IEnumerable<Assembly> DomainAssemblies { get; }
        Type EventStoreRepositoryType { get; }
    }
}