using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExampleDomain.Contacts.Queries.Models;
using ExampleDomain.Contacts.Queries.Repositories;
using ExampleDomain.Exceptions;

namespace CQRSMagic.Specifications.Steps.Support
{
    public class InMemoryContactRepository : IContactRepository
    {
        private readonly List<ContactReadModel> Contacts = new List<ContactReadModel>();

        public ContactReadModel GetByEmailAddress(string emailAddress)
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
    }
}