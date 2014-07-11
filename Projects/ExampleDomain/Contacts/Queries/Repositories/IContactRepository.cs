using System.Collections.Generic;
using System.Threading.Tasks;
using ExampleDomain.Contacts.Queries.Models;

namespace ExampleDomain.Contacts.Queries.Repositories
{
    public interface IContactRepository
    {
        Task<ContactReadModel> GetByEmailAddressAsync(string emailAddress);
        Task AddAsync(ContactReadModel contact);
        Task<IEnumerable<ContactReadModel>> FindAllAsync();
    }
}