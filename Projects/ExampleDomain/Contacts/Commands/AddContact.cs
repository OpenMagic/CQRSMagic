using System;
using CQRSMagic.Command;

namespace ExampleDomain.Contacts.Commands
{
    public class AddContact : CommandBase
    {
        public string Name { get; set; }
        public string EmailAddress { get; set; }
    }
}
