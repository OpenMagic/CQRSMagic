using AzureMagic;
using ExampleDomain.Contacts.Events;
using Microsoft.WindowsAzure.Storage.Table;

namespace ExampleDomain.Repositories.Azure.TableEntities
{
    public class ContactTableEntity : TableEntity
    {
        public ContactTableEntity()
        {
        }

        public ContactTableEntity(CreatedContact createdContact)
            : base(createdContact.AggregateId.ToPartitionKey(), string.Empty)
        {
            Name = createdContact.Name;
            EmailAddress = createdContact.EmailAddress;
        }

        public string Name { get; set; }
        public string EmailAddress { get; set; }
    }
}