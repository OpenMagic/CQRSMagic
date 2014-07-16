using System;
using System.Threading.Tasks;
using ExampleDomain.Contacts.Events;

namespace ExampleDomain.Contacts
{
    public interface IContactRepository
    {
        Task<ContactReadModel> GetContactAsync(Guid contactId);
        Task HandleEvent(ContactAdded @event);
    }
}