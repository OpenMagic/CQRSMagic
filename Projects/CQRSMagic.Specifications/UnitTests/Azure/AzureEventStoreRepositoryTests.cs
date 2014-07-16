using System;
using AzureMagic;
using CQRSMagic.Azure;
using CQRSMagic.Specifications.Support;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace CQRSMagic.Specifications.UnitTests.Azure
{
    public class AzureEventStoreRepositoryTests : IDisposable
    {
        protected const string ConnectionString = AzureStorage.DevelopmentConnectionString;
        protected string TableName = string.Format("Test{0}", Guid.NewGuid().ToString().Replace("-", ""));

        public void Dispose()
        {
            AzureStorage.DeleteTableIfExists(ConnectionString, TableName);
        }

        public class Constructor : AzureEventStoreRepositoryTests
        {
            [Fact]
            public void ShouldNotThrowException()
            {
                // When
                Action action = () => new AzureEventStoreRepository(ConnectionString, TableName, A.Fake<IAzureEventSerializer>());

                // Then
                action.ShouldNotThrow<Exception>();
            }

            [Fact]
            public void ShouldThrowExceptionWhen_connectionString_IsInvalid()
            {
                ActionShould.Throw<Exception>(() => new AzureEventStoreRepository("abc", "fake table name"));
            }

            [Fact]
            public void ShouldThrowExceptionWhen_tableName_IsInvalid()
            {
                ActionShould.Throw<Exception>(() => new AzureEventStoreRepository(ConnectionString, "!@#"));
            }

            [Fact]
            public void ShouldThrowArgumentNullExceptionWhen_connectionString_IsNull()
            {
                ActionShould.Throw<ArgumentNullException>("connectionString", () => new AzureEventStoreRepository(null, "fake table name"));
            }

            [Fact]
            public void ShouldThrowArgumentNullExceptionWhen_connectionString_IsEmpty()
            {
                ActionShould.Throw<ArgumentException>("connectionString", () => new AzureEventStoreRepository(string.Empty, "fake table name"));
            }

            [Fact]
            public void ShouldThrowArgumentNullExceptionWhen_tableName_IsNull()
            {
                ActionShould.Throw<ArgumentNullException>("tableName", () => new AzureEventStoreRepository("fake connection string", null));
            }

            [Fact]
            public void ShouldThrowArgumentNullExceptionWhen_tableName_IsEmpty()
            {
                ActionShould.Throw<ArgumentException>("tableName", () => new AzureEventStoreRepository("fake connection string", string.Empty));
            }

            [Fact]
            public void ShouldThrowArgumentNullExceptionWhen_serializer_IsNull()
            {
                ActionShould.Throw<ArgumentNullException>("serializer", () => new AzureEventStoreRepository("fake connection string", "fake table name", null));
            }
        }
    }
}