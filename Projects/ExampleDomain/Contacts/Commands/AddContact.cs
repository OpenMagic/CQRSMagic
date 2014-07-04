using System;
using CQRSMagic;

namespace ExampleDomain.Contacts.Commands
{
    // todo: validate properties
    public class AddContact : ICommand
    {
        public AddContact()
        {
            AggregateId = Guid.NewGuid();
        }

        public Guid AggregateId { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
    }
}