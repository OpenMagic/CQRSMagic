using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CQRSMagic.Commands;
using CQRSMagic.Domain;
using CQRSMagic.Events.Messaging;
using ExampleDomain.Contacts.Commands;
using ExampleDomain.Contacts.Events;
using OpenMagic.Exceptions;

namespace ExampleDomain.Contacts
{
    public class ContactAggregate : Aggregate,
        IHandleCommand<AddContact>,
        IApplyEvent<AddedContact>
    {
        static ContactAggregate()
        {
            // Create ICommand to IEvent maps.

            // todo: configure AutoMapper to handle AggregateType from ContactAggregate.
            Mapper.CreateMap<AddContact, AddedContact>()
                .ForMember(@event => @event.AggregateType, memberOptions => memberOptions.UseValue(typeof(ContactAggregate)));

            // Create IEvent to ContactAggregate maps.

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

        public Task<IEnumerable<IEvent>> HandleCommandAsync(AddContact command)
        {
            var addedContact = Mapper.Map<AddedContact>(command);

            return Task.FromResult((IEnumerable<IEvent>)new[] { addedContact });
        }
    }
}