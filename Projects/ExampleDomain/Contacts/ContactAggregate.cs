using CQRSMagic;

namespace ExampleDomain.Contacts
{
    public class ContactAggregate : Aggregate
    {
        public string Name { get; private set; }
        public string EmailAddress { get; private set; }
    }
}
