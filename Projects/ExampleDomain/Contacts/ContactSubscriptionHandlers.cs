using System.Threading.Tasks;
using AutoMapper;
using CQRSMagic.Events.Publishing;
using ExampleDomain.Contacts.Events;
using ExampleDomain.Contacts.Queries.Models;
using ExampleDomain.Contacts.Queries.Repositories;
using OpenMagic.Exceptions;

namespace ExampleDomain.Contacts
{
    public class ContactSubscriptionHandlers :
        ISubscribeTo<AddedContact>,
        ISubscribeTo<DeletedContact>
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

        public async Task HandleEventAsync(AddedContact @event)
        {
            var readModel = Mapper.Map<ContactReadModel>(@event);

            await Repository.AddContactAsync(readModel);
        }

        public async Task HandleEventAsync(DeletedContact @event)
        {
            await Repository.DeleteContactByIdAsync(@event.AggregateId);
        }
    }
}