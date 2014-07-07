using System.Collections.Generic;
using CQRSMagic.Events.Messaging;

namespace CQRSMagic.Commands
{
    public interface IHandleCommand<in TCommand> where TCommand : ICommand
    {
        IEnumerable<IEvent> HandleCommand(TCommand command);
    }
}