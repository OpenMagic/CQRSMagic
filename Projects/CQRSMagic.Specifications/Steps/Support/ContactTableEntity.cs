using Microsoft.WindowsAzure.Storage.Table;

namespace CQRSMagic.Specifications.Steps.Support
{
    public class ContactTableEntity : TableEntity
    {
        public string Name { get; set; }
        public string EmailAddress { get; set; }
    }
}