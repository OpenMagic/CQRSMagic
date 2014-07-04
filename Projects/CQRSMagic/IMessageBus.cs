using System.Collections.Generic;
using CQRSMagic.Commands;
using CQRSMagic.Events.Messaging;

namespace CQRSMagic
{
    public interface IMessageBus
    {
        IEnumerable<IEvent> SendCommand(ICommand command);
    }
}