using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExampleDomain.Contacts.Queries.Models;
using ExampleDomain.Contacts.Queries.Repositories;
using ExampleDomain.Exceptions;
using OpenMagic.Exceptions;

namespace CQRSMagic.Specifications.Steps.Support
{
    public class InMemoryContactRepository : IContactRepository
    {
        private readonly List<ContactReadModel> Contacts = new List<ContactReadModel>();

        public Task<ContactReadModel> GetByEmailAddressAsync(string emailAddress)
        {
            return Task.Run(() => GetByEmailAddress(emailAddress));
        }

        private ContactReadModel GetByEmailAddress(string emailAddress)
        {
            var contact = Contacts.SingleOrDefault(c => c.EmailAddress == emailAddress);

            if (contact != null)
            {
                return contact;
            }

            throw new ReadModelNotFoundException(string.Format("Cannot find contact by email address '{0}'.", emailAddress));
        }

        public Task AddAsync(ContactReadModel contact)
        {
            Contacts.Add(contact);

            return Task.FromResult(0);
        }

        public Task<IEnumerable<ContactReadModel>> FindAllAsync()
        {
            throw new ToDoException();
        }
    }
}