using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureMagic;
using CQRSMagic.Event;
using CQRSMagic.EventStorage;
using Microsoft.WindowsAzure.Storage.Table;

namespace CQRSMagic.Azure
{
    public class AzureEventStoreRepository : IEventStoreRepository
    {
        private readonly IAzureEventSerializer Serializer;
        private readonly AzureTableRepository<DynamicTableEntity> Repository;

        public AzureEventStoreRepository(CloudTableClient tableClient, string tableName, IAzureEventSerializer serializer)
        {
            Serializer = serializer;
            Repository = new AzureTableRepository<DynamicTableEntity>(tableClient, tableName);
        }

        public async Task<IEnumerable<IEvent>> FindEventsAsync(Guid aggregateId)
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

        public Task SaveEventsAsync(IEnumerable<IEvent> events)
        {
            var entities = events.Select(Serializer.Serialize);
            var tasks = entities.Select(Repository.AddEntityAsync);

            return Task.WhenAll(tasks);
        }
    }
}