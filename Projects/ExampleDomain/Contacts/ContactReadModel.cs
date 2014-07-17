using System;
using ExampleDomain.Repositories.Azure.TableEntities;

namespace ExampleDomain.Contacts
{
    public class ContactReadModel
    {
        public ContactReadModel(Guid id, string name, string emailAddress)
        {
            Id = id;
            Name = name;
            EmailAddress = emailAddress;
        }

        public ContactReadModel(ContactTableEntity tableEntity)
        {
            Id = new Guid(tableEntity.PartitionKey);
            Name = tableEntity.Name;
            EmailAddress = tableEntity.EmailAddress;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string EmailAddress { get; private set; }
    }
}