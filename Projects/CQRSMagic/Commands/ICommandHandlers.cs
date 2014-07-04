using System;
using System.Collections.Generic;
using CQRSMagic.Events;

namespace CQRSMagic.Commands
{
    public interface ICommandHandlers
    {
        Func<ICommand, IEnumerable<IEvent>> GetCommandHandlerFor(ICommand command);
    }
}