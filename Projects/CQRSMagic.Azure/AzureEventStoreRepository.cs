using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureMagic;
using CQRSMagic.Azure.Support;
using CQRSMagic.Event;
using CQRSMagic.EventStorage;
using Microsoft.WindowsAzure.Storage.Table;

namespace CQRSMagic.Azure
{
    public class AzureEventStoreRepository : IEventStoreRepository
    {
        private readonly AzureTableRepository<DynamicTableEntity> Repository;
        private readonly IAzureEventSerializer Serializer;

        public AzureEventStoreRepository()
            : this(IoC.Get<ISettings>())
        {
        }

        public AzureEventStoreRepository(ISettings settings)
            : this(settings.ConnectionString, settings.EventsTableName)
        {
        }

        public AzureEventStoreRepository(string connectionString, string eventsTableName)
            : this(connectionString, eventsTableName, IoC.Get<IAzureEventSerializer>())
        {
        }

        public AzureEventStoreRepository(string connectionString, string tableName, IAzureEventSerializer serializer)
        {
            Serializer = serializer;
            Repository = new AzureTableRepository<DynamicTableEntity>(connectionString, tableName);
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