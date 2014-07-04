using System.Collections.Generic;
using CQRSMagic.Events;

namespace CQRSMagic.Commands
{
    public interface IHandleCommand<in TCommand> where TCommand: ICommand
    {
        IEnumerable<IEvent> HandleCommand(TCommand command);
    }
}
