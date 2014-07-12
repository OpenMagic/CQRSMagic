using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CQRSMagic.Azure.Support;
using ExampleDomain.Contacts.Queries.Models;
using ExampleDomain.Contacts.Queries.Repositories;
using ExampleDomain.Repositories.Azure.TableEntities;
using Microsoft.WindowsAzure.Storage.Table;

namespace ExampleDomain.Repositories.Azure
{
    public class AzureContactRepository : IContactRepository
    {
        private readonly AzureTableRepository<ContactTableEntity> Repository;

        static AzureContactRepository()
        {
            Mapper.CreateMap<ContactReadModel, ContactTableEntity>()
                .ForMember(entity => entity.PartitionKey, memberOptions => memberOptions.MapFrom(contact => contact.Id.ToPartitionKey()));

            Mapper.CreateMap<ContactTableEntity, ContactReadModel>()
                .ForMember(contact => contact.Id, memberOptions => memberOptions.MapFrom(entity => new Guid(entity.PartitionKey)));
        }

        public AzureContactRepository(string connectionString, string tableName)
        {
            Repository = new AzureTableRepository<ContactTableEntity>(connectionString, tableName);
        }

        public async Task<ContactReadModel> GetContactByEmailAddressAsync(string emailAddress)
        {
            var filterCondition = TableQuery.GenerateFilterCondition("EmailAddress", QueryComparisons.Equal, emailAddress);
            var entities = await Repository.FindEntitiesWhereAsync(filterCondition);
            var entity = entities.Single();
            var readModel = Mapper.Map<ContactReadModel>(entity);

            return readModel;
        }

        public async Task AddContactAsync(ContactReadModel contact)
        {
            var entity = Mapper.Map<ContactTableEntity>(contact);

            await Repository.AddAsync(entity);
        }

        public Task DeleteContactByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ContactReadModel>> FindAllContactsAsync()
        {
            throw new NotImplementedException();
        }
    }
}