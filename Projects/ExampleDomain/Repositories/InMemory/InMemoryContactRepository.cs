using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExampleDomain.Contacts.Queries.Models;
using ExampleDomain.Contacts.Queries.Repositories;
using ExampleDomain.Exceptions;
using OpenMagic.Exceptions;

namespace ExampleDomain.Repositories.InMemory
{
    public class InMemoryContactRepository : IContactRepository
    {
        private readonly List<ContactReadModel> Contacts = new List<ContactReadModel>();

        public Task<ContactReadModel> GetContactByEmailAddressAsync(string emailAddress)
        {
            var contact = Contacts.SingleOrDefault(c => c.EmailAddress == emailAddress);

            if (contact != null)
            {
                return Task.FromResult(contact);
            }

            throw new ReadModelNotFoundException(string.Format("Cannot find contact by email address '{0}'.", emailAddress));
        }

        public Task AddContactAsync(ContactReadModel contact)
        {
            Contacts.Add(contact);

            return Task.FromResult(0);
        }

        public Task DeleteContactByIdAsync(Guid id)
        {
            Contacts.Remove(Contacts.Single(c => c.Id == id));
            return Task.FromResult(0);
        }

        public Task<IEnumerable<ContactReadModel>> FindAllContactsAsync()
        {
            return Task.FromResult(Contacts.AsEnumerable());
        }
    }
}