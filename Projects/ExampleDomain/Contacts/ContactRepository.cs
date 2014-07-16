using System;
using System.Threading.Tasks;
using OpenMagic.Exceptions;

namespace ExampleDomain.Contacts
{
    public class ContactRepository : IContactRepository
    {
        public Task<ContactReadModel> GetContactAsync(Guid contactId)
        {
            throw new ToDoException();
        }
    }
}