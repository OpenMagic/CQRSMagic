using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace CQRSMagic.Azure.Support
{
    // todo: move to AzureMagic.
    public class AzureTableRepository<TEntity> where TEntity : ITableEntity, new()
    {
        public readonly CloudTable Table;

        public AzureTableRepository(string connectionString, string tableName, bool createTableIfNotExists = true)
        {
            // todo: unit tests
            Table = AzureStorage.GetTable(connectionString, tableName);

            if (createTableIfNotExists)
            {
                Table.OnceOnlyCreateTableIfNotExists();
            }
        }

        public async Task AddAsync(TEntity entity)
        {
            // todo: unit tests
            var insert = TableOperation.Insert(entity);
            var result = await Table.ExecuteAsync(insert);

            if (result.HttpStatusCode == (int)HttpStatusCode.NoContent)
            {
                return;
            }

            throw new TableOperationException(string.Format("Cannot add {0}/{1}.", typeof(TEntity), entity.PartitionKey))
            {
                Data =
                {
                    {"TEntity", typeof(TEntity)},
                    {"result", result},
                    {"entity", entity}
                }
            };
        }

        public async Task<IEnumerable<TEntity>> GetEntitiesByPartitionKeyAsync(string partitionKey)
        {
            return await GetEntitiesWhereAsync(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));
        }

        public async Task<IEnumerable<TEntity>> GetEntitiesWhereAsync(string filterCondition)
        {
            try
            {
                var query = new TableQuery<TEntity>().Where(filterCondition);
                var items = new List<TEntity>();
                TableQuerySegment<TEntity> currentSegment = null;

                while (currentSegment == null || currentSegment.ContinuationToken != null)
                {
                    var token = currentSegment != null ? currentSegment.ContinuationToken : null;
                    currentSegment = await Table.ExecuteQuerySegmentedAsync(query, token);

                    items.AddRange(currentSegment.Results);
                }

                return items;
            }
            catch (Exception exception)
            {
                throw new TableOperationException(string.Format("Cannot get {0} entities where {1}.", typeof(TEntity), filterCondition), exception);
            }
        }
    }
}