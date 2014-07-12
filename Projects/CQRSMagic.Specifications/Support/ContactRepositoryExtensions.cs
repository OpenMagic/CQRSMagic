using System.Collections.Generic;
using ExampleDomain.Contacts.Queries.Models;
using ExampleDomain.Contacts.Queries.Repositories;

namespace CQRSMagic.Specifications.Support
{
    public static class ContactRepositoryExtensions
    {
        public static void AddContacts(this IContactRepository repository, IEnumerable<ContactReadModel> contacts)
        {
            foreach (var contact in contacts)
            {
                repository.AddContactAsync(contact).Wait();
            }
        }
    }
}
