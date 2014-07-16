using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExampleDomain.Contacts.Queries.Models;

namespace ExampleDomain.Contacts.Queries.Repositories
{
    public interface IContactRepository
    {
        Task<ContactReadModel> GetContactByEmailAddressAsync(string emailAddress);
        Task<ContactReadModel> GetContactById(Guid id);
        Task AddContactAsync(ContactReadModel contact);
        Task DeleteContactByIdAsync(Guid id);
        Task<IEnumerable<ContactReadModel>> FindAllContactsAsync();
    }
}