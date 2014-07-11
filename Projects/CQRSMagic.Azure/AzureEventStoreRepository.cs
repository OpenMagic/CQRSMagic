using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRSMagic.Azure.Support;
using CQRSMagic.Domain;
using CQRSMagic.Events.Messaging;
using CQRSMagic.Events.Sourcing.Repositories;
using Microsoft.Practices.ServiceLocation;
using Microsoft.WindowsAzure.Storage.Table;

namespace CQRSMagic.Azure
{
    public class AzureEventStoreRepository : IEventStoreRepository
    {
        private readonly AzureTableRepository<DynamicTableEntity> Repository;
        private readonly IAzureEventSerializer Serializer;

        public AzureEventStoreRepository(string connectionString, string tableName)
            : this(connectionString, tableName, ServiceLocator.Current.GetInstance<IAzureEventSerializer>())
        {
        }

        public AzureEventStoreRepository(string connectionString, string tableName, IAzureEventSerializer serializer)
        {
            Serializer = serializer;
            Repository = new AzureTableRepository<DynamicTableEntity>(connectionString, tableName);
        }

        public IEnumerable<IEvent> GetEvents<TAggregate>(Guid aggregateId) where TAggregate : IAggregate
        {
            var entities = Repository.FindEntitiesByPartitionKeyAsync(aggregateId.ToPartitionKey()).Result;
            var events = entities.Select(Serializer.Deserialize);

            return events;
        }

        public void SaveEvents(IEnumerable<IEvent> events)
        {
            foreach (var @event in events)
            {
                var entity = Serializer.Serialize(@event);

                Repository.AddAsync(entity).Wait();
            }
        }
    }
}