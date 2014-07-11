using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CQRSMagic.Events.Messaging;

namespace CQRSMagic.Commands
{
    public interface ICommandHandlers
    {
        Func<ICommand, Task<IEnumerable<IEvent>>> GetCommandHandlerFor(ICommand command);
    }
}