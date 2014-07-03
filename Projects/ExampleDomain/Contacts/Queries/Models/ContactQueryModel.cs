using System;

namespace ExampleDomain.Contacts.Queries.Models
{
    public class ContactQueryModel
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string EmailAddress { get; private set; }
    }
}