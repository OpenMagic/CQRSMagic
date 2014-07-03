using System.Collections.Generic;

namespace CQRSMagic
{
    public interface IMessageBus
    {
        IEnumerable<IEvent> Send(ICommand command);
    }
}