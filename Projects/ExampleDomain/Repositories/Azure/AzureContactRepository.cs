using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AzureMagic;
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
                .ForMember(entity => entity.PartitionKey, memberOptions => memberOptions.MapFrom(contact => contact.Id.ToPartitionKey()))
                .ForMember(entity => entity.RowKey, memberOptions => memberOptions.UseValue(""));

            Mapper.CreateMap<ContactTableEntity, ContactReadModel>()
                .ForMember(contact => contact.Id, memberOptions => memberOptions.MapFrom(entity => new Guid(entity.PartitionKey)));
        }

        public AzureContactRepository(string connectionString, string tableName)
        {
            Repository = new AzureTableRepository<ContactTableEntity>(connectionString, tableName);
        }

        private TableQuery<ContactTableEntity> Contacts
        {
            get { return Repository.Query(); }
        }

        public async Task<ContactReadModel> GetContactByEmailAddressAsync(string emailAddress)
        {
            return await GetContact(
                from contact in Contacts
                where contact.EmailAddress == emailAddress
                select contact
                );
        }

        public async Task<ContactReadModel> GetContactById(Guid id)
        {
            var tableEntity = await GetTableEntityById(id);
            var contact = Mapper.Map<ContactReadModel>(tableEntity);

            return contact;
        }

        public async Task AddContactAsync(ContactReadModel contact)
        {
            var entity = Mapper.Map<ContactTableEntity>(contact);

            await Repository.AddEntity(entity);
        }

        public async Task DeleteContactByIdAsync(Guid id)
        {
            var tableEntity = await GetTableEntityById(id);

            await Repository.DeleteEntity(tableEntity);
        }

        public async Task<IEnumerable<ContactReadModel>> FindAllContactsAsync()
        {
            var tableEntities = await Repository.Query().ExecuteAsync();
            var contacts = tableEntities.Select(Mapper.Map<ContactReadModel>);

            return contacts;
        }

        private async Task<ContactReadModel> GetContact(IQueryable<ContactTableEntity> tableQuery)
        {
            var contacts = await FindContacts(tableQuery);

            return contacts.Single();
        }

        private async Task<ContactTableEntity> GetTableEntityById(Guid id)
        {
            var partitionKey = id.ToPartitionKey();
            var tableEntity = await GetTableEntity(
                from contact in Contacts
                where contact.PartitionKey == partitionKey
                select contact);

            return tableEntity;
        }

        private async Task<ContactTableEntity> GetTableEntity(IQueryable<ContactTableEntity> tableQuery)
        {
            var tableEntities = await FindTableEntities(tableQuery);
            var tableEntity = tableEntities.Single();

            return tableEntity;
        }

        private async Task<IEnumerable<ContactTableEntity>> FindTableEntities(IQueryable<ContactTableEntity> tableQuery)
        {
            var tableEntities = await tableQuery.ExecuteAsync();

            return tableEntities;
        }

        private async Task<IEnumerable<ContactReadModel>> FindContacts(IQueryable<ContactTableEntity> tableQuery)
        {
            var tableEntities = await FindTableEntities(tableQuery);
            var contacts = tableEntities.Select(Mapper.Map<ContactReadModel>);

            return contacts;
        }
    }
}