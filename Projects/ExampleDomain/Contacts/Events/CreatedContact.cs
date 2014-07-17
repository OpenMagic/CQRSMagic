using System;
using CQRSMagic.Event;
using ExampleDomain.Contacts.Commands;

namespace ExampleDomain.Contacts.Events
{
    public class CreatedContact : EventBase
    {
        protected CreatedContact()
        {
            // AzureEventSerializer requires a parameterless constructor.
        }

        public CreatedContact(Guid contactId, string name, string emailAddress)
            : base(contactId)
        {
            Name = name;
            EmailAddress = emailAddress;
        }

        public CreatedContact(CreateContact contact)
            : this(contact.Id, contact.Name, contact.EmailAddress)
        {
        }

        public string Name { get; private set; }
        public string EmailAddress { get; private set; }
    }
}