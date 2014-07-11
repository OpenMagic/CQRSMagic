using System.Threading.Tasks;
using AutoMapper;
using CQRSMagic.Events.Publishing;
using ExampleDomain.Contacts.Events;
using ExampleDomain.Contacts.Queries.Models;
using ExampleDomain.Contacts.Queries.Repositories;

namespace ExampleDomain.Contacts
{
    public class ContactSubscriptionHandlers :
        ISubscribeTo<AddedContact>
    {
        private readonly IContactRepository Repository;

        static ContactSubscriptionHandlers()
        {
            // todo: configure AutoMapper to handle AggregateId to Id.
            Mapper.CreateMap<AddedContact, ContactReadModel>()
                .ForMember(contactReadModel => contactReadModel.Id, memberOptions => memberOptions.MapFrom(contactAdded => contactAdded.AggregateId));
        }

        public ContactSubscriptionHandlers(IContactRepository repository)
        {
            Repository = repository;
        }

        public Task HandleEventAsync(AddedContact @event)
        {
            var readModel = Mapper.Map<ContactReadModel>(@event);

            return Repository.AddAsync(readModel);
        }
    }
}