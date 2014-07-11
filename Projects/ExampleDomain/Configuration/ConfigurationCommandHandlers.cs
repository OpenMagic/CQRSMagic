using System.Collections.Generic;
using System.Threading.Tasks;
using CQRSMagic.Commands;
using CQRSMagic.Events.Messaging;
using ExampleDomain.Configuration.Commands;
using ExampleDomain.Contacts.Queries.Repositories;
using OpenMagic.Exceptions;

namespace ExampleDomain.Configuration
{
    public class ConfigurationCommandHandlers : 
        IHandleCommand<ClearAll>
    {
        private readonly IContactRepository ContactRepository;

        public ConfigurationCommandHandlers(IContactRepository contactRepository)
        {
            ContactRepository = contactRepository;
        }

        public Task<IEnumerable<IEvent>> HandleCommandAsync(ClearAll command)
        {
            throw new ToDoException();
            //var contacts = await ContactRepository.FindAllAsync();
            //var events = contacts.Select(contact => new DeletedContact(contact));

            //return events;
        }
    }
}
