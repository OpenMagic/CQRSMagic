using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRSMagic.Azure.Support;
using CQRSMagic.Specifications.Support;
using FluentAssertions;
using Microsoft.WindowsAzure.Storage.Table;
using OpenMagic.Exceptions;
using Xunit;

namespace CQRSMagic.Specifications.UnitTests.Azure.Support
{
    public class AzureTableRepositoryTests : IDisposable
    {
        protected const string ConnectionString = AzureStorage.DevelopmentConnectionString;
        protected string TableName = string.Format("Test{0}", Guid.NewGuid().ToString().Replace("-", ""));

        public void Dispose()
        {
            AzureStorage.DeleteTableIfExists(ConnectionString, TableName);
        }

        public class AddAsync : AzureTableRepositoryTests
        {
            [Fact]
            public void ShouldThrowArgumentNullExceptionIf_entity_IsNull()
            {
                // Given
                var repository = new AzureTableRepository<DynamicTableEntity>(ConnectionString, TableName);

                // Then
                ActionShould.Throw<ArgumentNullException>("entity", () => repository.AddAsync(null));
            }

            [Fact]
            public void ShouldAddEntityToTable()
            {
                // Given
                var id = Guid.NewGuid();
                var repository = new AzureTableRepository<DynamicTableEntity>(ConnectionString, TableName);
                var entity = new DynamicTableEntity(id.ToPartitionKey(), DateTimeOffset.Now.ToRowKey());

                entity.Properties["FakePropertyName"] = new EntityProperty("FakePropertyValue");

                // When
                repository.AddAsync(entity).Wait();

                // Then
                var table = AzureStorage.GetTable(ConnectionString, TableName);
                var result = table.Execute(TableOperation.Retrieve<DynamicTableEntity>(entity.PartitionKey, entity.RowKey)).Result;
                var retrievedEntity = (DynamicTableEntity) result;

                retrievedEntity["FakePropertyName"].StringValue.Should().Be("FakePropertyValue");
            }
        }

        public class GetEntitiesByPartitionKeyAsync : AzureTableRepositoryTests
        {
            [Fact]
            public void ShouldThrowArgumentNullExceptionIf_partitionKey_IsNull()
            {
                // Given
                var repository = new AzureTableRepository<DynamicTableEntity>(ConnectionString, TableName);

                // Then
                ActionShould.Throw<ArgumentNullException>("partitionKey", () => repository.FindEntitiesByPartitionKeyAsync(null));
            }

            [Fact]
            public void ShouldThrowArgumentNullExceptionIf_partitionKey_IsEmpty()
            {
                // Given
                var repository = new AzureTableRepository<DynamicTableEntity>(ConnectionString, TableName);

                // Then
                ActionShould.Throw<ArgumentException>("partitionKey", () => repository.FindEntitiesByPartitionKeyAsync(string.Empty));
            }

            [Fact]
            public void ShouldReturnEmptyEnumerableWhen_partitionKey_DoesNotExist()
            {
                // Given
                var repository = new AzureTableRepository<DynamicTableEntity>(ConnectionString, TableName);

                // When
                var entities = repository.FindEntitiesByPartitionKeyAsync("partitionkey that does not exist").Result;

                // Then
                entities.Any().Should().BeFalse();
            }

            [Fact]
            public void ShouldReturnEnumerableOfEntitiesMatching_partitionKey()
            {
                // Given
                const string partitionKey = "a";

                var expectedTableEntities = Enumerable.Range(1, 10).Select(i =>
                {
                    var entity = new DynamicTableEntity
                    {
                        PartitionKey = partitionKey,
                        RowKey = i.ToRowKey(),
                        Properties = new Dictionary<string, EntityProperty>
                        {
                            {"FakePropertyName", new EntityProperty(string.Format("FakePropertyValue{0}", i))}
                        }
                    };

                    return entity;
                }).ToArray();

                var repository = new AzureTableRepository<DynamicTableEntity>(ConnectionString, TableName);
                var addTasks = expectedTableEntities.Select(repository.AddAsync).ToArray();

                Task.WaitAll(addTasks);

                // When
                var actualTableEntities = repository.FindEntitiesByPartitionKeyAsync(partitionKey).Result.ToArray();

                // Then
                var expectedPropertyValues = expectedTableEntities.Select(e=> e.Properties["FakePropertyName"].StringValue);
                var actualPropertyValues = actualTableEntities.Select(e => e.Properties["FakePropertyName"].StringValue);

                expectedPropertyValues.ShouldBeEquivalentTo(actualPropertyValues);
            }
        }
    }
}