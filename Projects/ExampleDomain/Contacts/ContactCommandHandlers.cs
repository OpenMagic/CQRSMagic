using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CQRSMagic.Commands;
using CQRSMagic.Domain;
using CQRSMagic.Events.Messaging;
using ExampleDomain.Contacts.Commands;
using ExampleDomain.Contacts.Events;

namespace ExampleDomain.Contacts
{
    public class ContactCommandHandlers : Aggregate,
        IHandleCommand<AddContact>
    {
        static ContactCommandHandlers()
        {
            // todo: configure AutoMapper to handle AggregateType from ContactAggregate.
            Mapper.CreateMap<AddContact, AddedContact>()
                .ForMember(@event => @event.AggregateType, memberOptions => memberOptions.UseValue(typeof(ContactAggregate)));
        }

        public Task<IEnumerable<IEvent>> HandleCommandAsync(AddContact command)
        {
            var addedContact = Mapper.Map<AddedContact>(command);

            return Task.FromResult((IEnumerable<IEvent>)new[] { addedContact });
        }
    }
}