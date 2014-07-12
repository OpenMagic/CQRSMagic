using System;

namespace ExampleDomain.Contacts.Queries.Models
{
    public class ContactReadModel
    {
        public ContactReadModel()
        {
        }

        public ContactReadModel(Guid id, string name, string emailAddress)
        {
            Id = id;
            Name = name;
            EmailAddress = emailAddress;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string EmailAddress { get; private set; }
    }
}