using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using CQRSMagic.EventStorage;
using ExampleDomain.Contacts;
using ExampleDomain.Contacts.Events;

namespace ExampleDomain.Repositories.InMemory
{
    public class InMemoryContactRepository : IContactRepository
    {
        private readonly ConcurrentDictionary<Guid, ContactReadModel> Contacts;
        private readonly IEventStore EventStore;

        public InMemoryContactRepository(IEventStore eventStore)
        {
            EventStore = eventStore;
            Contacts = new ConcurrentDictionary<Guid, ContactReadModel>();
        }

        public Task<ContactReadModel> GetContactAsync(Guid contactId)
        {
            return Task.FromResult(Contacts[contactId]);
        }

        public async Task HandleEvent(ContactAdded @event)
        {
            var aggregate = await EventStore.GetAggregateAsync<ContactAggregate>(@event.AggregateId);
            var readModel = new ContactReadModel(aggregate.Id, aggregate.Name, aggregate.EmailAddress);

            if (Contacts.TryAdd(readModel.Id, readModel))
            {
                //return Task.FromResult(true);
                return;
            }

            throw new Exception(string.Format("Cannot add Contacts/{0}. Probably a duplicate.", readModel.Id));
        }
    }
}