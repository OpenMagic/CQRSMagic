using System.Collections.Generic;
using System.Threading.Tasks;
using CQRSMagic.Command;
using CQRSMagic.Event;
using ExampleDomain.Contacts.Commands;
using ExampleDomain.Contacts.Events;

namespace ExampleDomain.Contacts
{
    public class ContactCommandHandlers :
        IHandleCommand<AddContact>
    {
        public Task<IEnumerable<IEvent>> HandleCommand(AddContact command)
        {
            return Task.FromResult((IEnumerable<IEvent>) new IEvent[] {new ContactAdded(command)});
        }
    }
}