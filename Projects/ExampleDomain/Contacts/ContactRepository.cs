using System;
using System.Threading.Tasks;

namespace ExampleDomain.Contacts
{
    public class ContactRepository : IContactRepository
    {
        public Task<ContactReadModel> GetContactAsync(Guid contactId)
        {
            throw new NotImplementedException();
        }
    }
}