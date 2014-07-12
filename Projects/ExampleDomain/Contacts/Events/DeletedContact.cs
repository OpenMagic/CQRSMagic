using System;
using CQRSMagic.Events.Messaging;

namespace ExampleDomain.Contacts.Events
{
    public class DeletedContact : IEvent
    {
        public DeletedContact()
        {
            EventCreated = DateTime.UtcNow;
        }

        public DeletedContact(Type aggregateType, Guid aggregateId)
            : this()
        {
            AggregateType = aggregateType;
            AggregateId = aggregateId;
        }

        public Type AggregateType { get; private set; }
        public Guid AggregateId { get; private set; }
        public DateTimeOffset EventCreated { get; private set; }
    }
}