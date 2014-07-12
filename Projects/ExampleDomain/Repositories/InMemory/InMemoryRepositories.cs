using CQRSMagic.Events.Sourcing.Repositories;
using ExampleDomain.Contacts.Queries.Repositories;

namespace ExampleDomain.Repositories.InMemory
{
    public class InMemoryRepositories : IRepositoryFactory
    {
        public InMemoryRepositories()
        {
            EventStoreRepository = new InMemoryEventStoreRepository();
            ContactRepository = new InMemoryContactRepository();
        }

        public IEventStoreRepository EventStoreRepository { get; private set; }
        public IContactRepository ContactRepository { get; private set; }
    }
}