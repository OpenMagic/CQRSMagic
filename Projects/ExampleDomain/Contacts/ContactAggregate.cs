using System;

namespace ExampleDomain.Contacts
{
    public class ContactAggregate
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string EmailAddress { get; private set; }
    }
}