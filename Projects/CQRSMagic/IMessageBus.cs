using System.Collections.Generic;
using CQRSMagic.Events;

namespace CQRSMagic
{
    public interface IMessageBus
    {
        IEnumerable<IEvent> SendCommand(ICommand command);
    }
}