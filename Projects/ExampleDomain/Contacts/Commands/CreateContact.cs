using CQRSMagic.Command;

namespace ExampleDomain.Contacts.Commands
{
    public class CreateContact : CommandBase
    {
        public string Name { get; set; }
        public string EmailAddress { get; set; }
    }
}