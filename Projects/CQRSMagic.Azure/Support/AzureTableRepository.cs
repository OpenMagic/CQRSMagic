using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Anotar.CommonLogging;
using Microsoft.WindowsAzure.Storage.Table;
using NullGuard;

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
            try
            {
                LogTo.Debug("Adding {0}/{1}/{2}.", typeof(TEntity), entity.PartitionKey, entity.RowKey);

                var insert = TableOperation.Insert(entity);
                var result = await Table.ExecuteAsync(insert);

                if (result.HttpStatusCode == (int)HttpStatusCode.NoContent)
                {
                    return;
                }

                throw new Exception(string.Format("Cannot add {0}/{1}/{2}.", typeof(TEntity), entity.PartitionKey, entity.RowKey))
                {
                    Data =
                    {
                        {"TEntity", typeof(TEntity)},
                        {"result", result},
                        {"entity", entity}
                    }
                };
            }
            catch (Exception exception)
            {
                LogTo.ErrorException(exception.Message, exception);
                throw;
            }
        }

        public async Task<IEnumerable<TEntity>> FindEntitiesAsync()
        {
            return await FindEntitiesWhereAsync(null);
        }

        public async Task<IEnumerable<TEntity>> FindEntitiesByPartitionKeyAsync(string partitionKey)
        {
            return await FindEntitiesWhereAsync(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));
        }

        public async Task<IEnumerable<TEntity>> FindEntitiesWhereAsync([AllowNull]string filterCondition)
        {
            try
            {
                var query = new TableQuery<TEntity>();

                if (filterCondition != null)
                {
                    query = query.Where(filterCondition);
                }

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
                var message = string.Format("Cannot get {0} entities where {1}.", typeof(TEntity), filterCondition);
                LogTo.ErrorException(message, exception);
                throw new Exception(message, exception);
            }
        }
    }
}