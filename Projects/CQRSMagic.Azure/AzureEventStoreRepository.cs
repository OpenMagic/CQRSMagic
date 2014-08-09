using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureMagic.Tables;
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

        public AzureEventStoreRepository(string connectionString, string eventsTableName, IAzureEventSerializer serializer)
            : this(connectionString, eventsTableName, serializer, IoC.Get<IAzureTableRepositoryLogger>())
        {
        }

        public AzureEventStoreRepository(string connectionString, string eventsTableName, IAzureEventSerializer serializer, IAzureTableRepositoryLogger logger)
        {
            Serializer = serializer;
            Repository = new AzureTableRepository<DynamicTableEntity>(connectionString, eventsTableName, logger);
        }

        public async Task<IEnumerable<IEvent>> FindAllEventsAsync()
        {
            var tableEntities = await Repository.Query().ExecuteAsync();
            var events = tableEntities.Select(Serializer.Deserialize);

            return events;
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
            return SaveEventsAsync(events.ToArray());
        }

        private Task SaveEventsAsync(IEvent[] events)
        {
            if (events.Length > Serializer.MaximumEventsPerTransaction)
            {
                throw new ArgumentException(string.Format("Cannot save more than {0:N0} events in one transaction.", Serializer.MaximumEventsPerTransaction));
            }

            var entities = events.Select((@event, index) => Serializer.Serialize(@event, index));
            var tasks = entities.Select(Repository.AddEntityAsync);

            return Task.WhenAll(tasks);
        }
    }
}