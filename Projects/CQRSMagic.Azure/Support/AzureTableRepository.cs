using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
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
                var insert = TableOperation.Insert(entity);
                var result = await Table.ExecuteAsync(insert);

                if (result.HttpStatusCode == (int)HttpStatusCode.NoContent)
                {
                    return;
                }

                throw new Exception(string.Format("Cannot add {0}/{1}/{2}. HttpStatusCode {3}.", Table.Name, entity.PartitionKey, entity.RowKey, result.HttpStatusCode));
            }
            catch (Exception exception)
            {
                throw new Exception(string.Format("Cannot add {0}/{1}/{2}.", Table.Name, entity.PartitionKey, entity.RowKey), exception);
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
                throw new Exception(string.Format("Cannot get entities from {0} where {1}.", Table.Name, filterCondition), exception);
            }
        }
    }
}