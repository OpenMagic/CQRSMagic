using System;
using System.Collections.Generic;
using CQRSMagic.WebApiExample.Products;

namespace CQRSMagic.WebApiExample
{
    internal class ServiceLocator
    {
        internal static readonly IEventStore EventStore = new InMemoryEventStore();
        internal static readonly IEventPublisher EventPublisher = new EventPublisher();
        internal static readonly Dictionary<Guid, ProductReadModel> ProductReadModels = new Dictionary<Guid, ProductReadModel>();
    }
}