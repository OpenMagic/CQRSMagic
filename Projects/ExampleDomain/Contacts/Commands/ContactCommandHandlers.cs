﻿using System.Collections.Generic;
using System.Threading.Tasks;
using CQRSMagic.Command;
using CQRSMagic.Event;
using ExampleDomain.Contacts.Events;

namespace ExampleDomain.Contacts.Commands
{
    public class ContactCommandHandlers :
        IHandleCommand<CreateContact>
    {
        public Task<IEnumerable<IEvent>> HandleCommandAsync(CreateContact command)
        {
            return Task.FromResult((IEnumerable<IEvent>) new IEvent[] {new CreatedContact(command)});
        }
    }
}