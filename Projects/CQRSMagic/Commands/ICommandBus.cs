using System.Collections.Generic;
using CQRSMagic.Events.Messaging;

namespace CQRSMagic.Commands
{
    public interface ICommandBus
    {
        IEnumerable<IEvent> SendCommand(ICommand command);
    }
}