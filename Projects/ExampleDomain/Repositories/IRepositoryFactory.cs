using CQRSMagic.Events.Sourcing.Repositories;
using ExampleDomain.Contacts.Queries.Repositories;

namespace ExampleDomain.Repositories
{
    public interface IRepositoryFactory
    {
        IEventStoreRepository EventStoreRepository { get; }
        IContactRepository ContactRepository { get; }
    }
}