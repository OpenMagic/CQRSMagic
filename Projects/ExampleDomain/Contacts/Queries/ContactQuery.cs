using System.Threading.Tasks;
using AutoMapper;
using CQRSMagic.Events.Publishing;
using ExampleDomain.Contacts.Events;
using ExampleDomain.Contacts.Queries.Models;
using ExampleDomain.Contacts.Queries.Repositories;

namespace ExampleDomain.Contacts.Queries
{
    public class ContactQuery : 
        ISubscribeTo<ContactAdded>
    {
        private readonly IContactRepository Repository;

        static ContactQuery()
        {
            // todo: configure AutoMapper to handle AggregateId to Id.
            Mapper.CreateMap<ContactAdded, ContactReadModel>()
                .ForMember(contactReadModel => contactReadModel.Id, memberOptions => memberOptions.MapFrom(contactAdded => contactAdded.AggregateId));
        }

        public ContactQuery(IContactRepository repository)
        {
            Repository = repository;
        }

        public ContactReadModel GetByEmailAddress(string emailAddress)
        {
            return Repository.GetByEmailAddress(emailAddress);
        }

        public Task HandleEventAsync(ContactAdded @event)
        {
            var readModel = Mapper.Map<ContactReadModel>(@event);

            return Repository.AddAsync(readModel);
        }
    }
}