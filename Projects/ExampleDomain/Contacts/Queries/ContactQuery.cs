using ExampleDomain.Contacts.Queries.Models;
using OpenMagic.Exceptions;

namespace ExampleDomain.Contacts.Queries
{
    public class ContactQuery
    {
        public ContactQueryModel GetByEmailAddress(string contactEmailAddress)
        {
            throw new ToDoException();
        }
    }
}