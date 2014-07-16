using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureMagic;
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

        public async Task<IEnumerable<IEvent>> GetEventsAsync<TAggregate>(Guid aggregateId) where TAggregate : IAggregate
        {
            var partitionKey = aggregateId.ToPartitionKey();
            var tableEntities = await (
                from entity in Repository.Query()
                where entity.PartitionKey == partitionKey
                select entity
                ).ExecuteAsync();

            var events = tableEntities.Select(Serializer.Deserialize);

            return events;
        }

        public async Task SaveEventsAsync(IEnumerable<IEvent> events)
        {
            // todo: unit tests
            var entities = events.Select(Serializer.Serialize);
            var tasks = entities.Select(Repository.AddEntity);

            await Task.WhenAll(tasks.ToArray());
        }
    }
}