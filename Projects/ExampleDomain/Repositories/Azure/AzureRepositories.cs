using AzureMagic;
using CQRSMagic.Azure;
using CQRSMagic.Events.Sourcing.Repositories;
using ExampleDomain.Contacts.Queries.Repositories;

namespace ExampleDomain.Repositories.Azure
{
    public class AzureRepositories : IRepositoryFactory
    {
        public AzureRepositories()
            : this(AzureStorage.DevelopmentConnectionString, "CQRSMagicExampleMVCApplication")
        {
        }

        private AzureRepositories(string connectionString, string tableNamePrefix)
        {
            EventStoreRepository = new AzureEventStoreRepository(connectionString, tableNamePrefix + "EventStore");
            ContactRepository = new AzureContactRepository(connectionString, tableNamePrefix + "Contacts");
        }

        public IEventStoreRepository EventStoreRepository { get; private set; }
        public IContactRepository ContactRepository { get; private set; }
    }
}