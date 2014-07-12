using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CQRSMagic.Commands;
using CQRSMagic.Events.Messaging;
using ExampleDomain.Configuration.Commands;
using ExampleDomain.Contacts;
using ExampleDomain.Contacts.Events;
using ExampleDomain.Contacts.Queries.Models;
using ExampleDomain.Contacts.Queries.Repositories;

namespace ExampleDomain.Configuration
{
    public class ConfigurationCommandHandlers : 
        IHandleCommand<ClearAll>
    {
        private readonly IContactRepository ContactRepository;

        static ConfigurationCommandHandlers()
        {
            // todo: configure AutoMapper to handle AggregateType from ContactAggregate.
            // todo: configure AutoMapper to handle AggregateType from ContactAggregate.
            Mapper.CreateMap<ContactReadModel, DeletedContact>()
                .ForMember(@event => @event.AggregateType, memberOptions => memberOptions.UseValue(typeof(ContactAggregate)))
                .ForMember(aggregate => aggregate.AggregateId, memberOptions => memberOptions.MapFrom(src => src.Id));
        }

        public ConfigurationCommandHandlers(IContactRepository contactRepository)
        {
            ContactRepository = contactRepository;
        }

        public async Task<IEnumerable<IEvent>> HandleCommandAsync(ClearAll command)
        {
            var contacts = await ContactRepository.FindAllContactsAsync();
            var events = contacts.Select(Mapper.Map<DeletedContact>).ToArray();

            return events;
        }
    }
}
