using System;
using CQRSMagic.Events.Messaging;
using ExampleDomain.Contacts.Queries.Models;
using OpenMagic.Exceptions;

namespace ExampleDomain.Contacts.Events
{
    public class DeletedContact : IEvent
    {
        public DeletedContact(ContactReadModel contact)
        {
            throw new ToDoException();
        }

        public Type AggregateType { get; private set; }
        public Guid AggregateId { get; private set; }
        public DateTimeOffset EventCreated { get; private set; }
    }
}