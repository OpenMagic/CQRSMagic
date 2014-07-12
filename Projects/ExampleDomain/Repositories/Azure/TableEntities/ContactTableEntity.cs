using Microsoft.WindowsAzure.Storage.Table;

namespace ExampleDomain.Repositories.Azure.TableEntities
{
    public class ContactTableEntity : TableEntity
    {
        public string Name { get; set; }
        public string EmailAddress { get; set; }
    }
}