using System;
using System.Threading.Tasks;

namespace ExampleDomain.Contacts
{
    public interface IContactRepository
    {
        Task<ContactReadModel> GetContactAsync(Guid contactId);
    }
}