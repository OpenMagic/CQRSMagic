using AutoMapper;
using CQRSMagic.Domain;
using CQRSMagic.Events.Messaging;
using ExampleDomain.Contacts.Events;
using OpenMagic.Exceptions;

namespace ExampleDomain.Contacts
{
    public class ContactAggregate : Aggregate,
        IApplyEvent<AddedContact>,
        IApplyEvent<DeletedContact>
    {
        static ContactAggregate()
        {
            // todo: configure AutoMapper to handle AggregateId to Id.
            Mapper.CreateMap<AddedContact, ContactAggregate>()
                .ForMember(aggregate => aggregate.Id, memberOptions => memberOptions.MapFrom(src => src.AggregateId));
        }

        public string Name { get; private set; }
        public string EmailAddress { get; private set; }

        public void ApplyEvent(AddedContact @event)
        {
            Mapper.Map(@event, this);
        }

        public void ApplyEvent(DeletedContact @event)
        {
            throw new ToDoException();
        }
    }
}