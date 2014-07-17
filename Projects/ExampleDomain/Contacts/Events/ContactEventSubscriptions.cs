using System.Threading.Tasks;
using CQRSMagic.Event;

namespace ExampleDomain.Contacts.Events
{
    public class ContactEventSubscriptions :
        ISubscribeTo<ContactAdded>
    {
        private readonly IContactRepository Repository;

        public ContactEventSubscriptions(IContactRepository repository)
        {
            Repository = repository;
        }

        public Task HandleEvent(ContactAdded @event)
        {
            return Repository.AddContactAsync(@event);
        }
    }
}