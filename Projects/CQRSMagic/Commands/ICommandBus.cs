using System.Collections.Generic;
using CQRSMagic.Events;

namespace CQRSMagic.Commands
{
    public interface ICommandBus
    {
        IEnumerable<IEvent> SendCommand(ICommand command);
    }
}
