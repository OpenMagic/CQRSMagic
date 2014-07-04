using System.Threading.Tasks;
using ExampleDomain.Contacts.Queries.Models;

namespace ExampleDomain.Contacts.Queries.Repositories
{
    public interface IContactRepository
    {
        ContactReadModel GetByEmailAddress(string emailAddress);
        Task AddAsync(ContactReadModel contact);
    }
}