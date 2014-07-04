using System;
using CQRSMagic.Events;

namespace ExampleDomain.Contacts.Events
{
    public class ContactAdded : IEvent
    {
        public Type AggregateType { get; private set; }
        public Guid AggregateId { get; private set; }
        public string Name { get; private set; }
        public string EmailAddress { get; private set; }
    }
}