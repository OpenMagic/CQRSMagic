using System.Collections.Generic;
using AutoMapper;
using CQRSMagic.Commands;
using CQRSMagic.Domain;
using CQRSMagic.Events.Messaging;
using ExampleDomain.Contacts.Commands;
using ExampleDomain.Contacts.Events;

namespace ExampleDomain.Contacts
{
    public class ContactAggregate : Aggregate,
        IHandleCommand<AddContact>,
        IApplyEvent<ContactAdded>
    {
        static ContactAggregate()
        {
            // Create ICommand to IEvent maps.
            
            // todo: configure AutoMapper to handle AggregateType from ContactAggregate.
            Mapper.CreateMap<AddContact, ContactAdded>()
                .ForMember(@event => @event.AggregateType, memberOptions => memberOptions.UseValue(typeof(ContactAggregate)));

            // Create IEvent to ContactAggregate maps.

            // todo: configure AutoMapper to handle AggregateId to Id.
            Mapper.CreateMap<ContactAdded, ContactAggregate>()
                .ForMember(aggregate => aggregate.Id, memberOptions => memberOptions.MapFrom(src => src.AggregateId));
        }

        public string Name { get; private set; }
        public string EmailAddress { get; private set; }

        public void ApplyEvent(ContactAdded @event)
        {
            Mapper.Map(@event, this);
        }

        public IEnumerable<IEvent> HandleCommand(AddContact command)
        {
            var contactAdded = Mapper.Map<ContactAdded>(command);

            yield return contactAdded;
        }
    }
}