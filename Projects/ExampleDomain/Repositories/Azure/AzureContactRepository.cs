﻿using System;
using System.Linq;
using System.Threading.Tasks;
using AzureMagic;
using ExampleDomain.Contacts;
using ExampleDomain.Contacts.Events;
using ExampleDomain.Repositories.Azure.TableEntities;
using Microsoft.WindowsAzure.Storage.Table;

namespace ExampleDomain.Repositories.Azure
{
    public class AzureContactRepository : IContactRepository
    {
        private readonly AzureTableRepository<ContactTableEntity> Repository;

        public AzureContactRepository(CloudTableClient tableClient, string tableName)
        {
            Repository = new AzureTableRepository<ContactTableEntity>(tableClient, tableName);
        }

        public async Task<ContactReadModel> GetContactAsync(Guid contactId)
        {
            var partitionKey = contactId.ToPartitionKey();
            var query = await (
                from contact in Repository.Query()
                where contact.PartitionKey == partitionKey
                select contact).ExecuteAsync();

            var contacts = query.Select(c => new ContactReadModel(c));

            return contacts.Single();
        }

        public Task AddContactAsync(CreatedContact @event)
        {
            return Repository.AddEntityAsync(new ContactTableEntity(@event));
        }
    }
}