using System;
using CQRSMagic.Event;

namespace ExampleDomain.Contacts.Events
{
    public class ContactAdded : EventBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
    }
}
