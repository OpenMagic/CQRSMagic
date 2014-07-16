using System;
using CQRSMagic.Event;
using ExampleDomain.Contacts.Commands;

namespace ExampleDomain.Contacts.Events
{
    public class ContactAdded : EventBase
    {
        public ContactAdded(Guid contactId, string name, string emailAddress)
        {
            AggregateId = contactId;
            Name = name;
            EmailAddress = emailAddress;
        }

        public ContactAdded(AddContact contact)
            : this(contact.Id, contact.Name, contact.EmailAddress)
        {
        }

        public string Name { get; private set; }
        public string EmailAddress { get; private set; }
    }
}