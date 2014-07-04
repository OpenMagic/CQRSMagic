using System;

namespace ExampleDomain.Contacts.Queries.Models
{
    public class ContactReadModel
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string EmailAddress { get; private set; }
    }
}