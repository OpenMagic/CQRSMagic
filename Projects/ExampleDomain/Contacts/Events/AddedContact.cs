using System;
using CQRSMagic.Events.Messaging;

namespace ExampleDomain.Contacts.Events
{
    public class AddedContact : IEvent
    {
        public AddedContact()
        {
            EventCreated = DateTime.UtcNow;
        }

        public Type AggregateType { get; private set; }
        public Guid AggregateId { get; private set; }
        public DateTimeOffset EventCreated { get; private set; }

        public string Name { get; private set; }
        public string EmailAddress { get; private set; }
    }
}