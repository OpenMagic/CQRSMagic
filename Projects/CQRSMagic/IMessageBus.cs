using System.Collections.Generic;
using System.Threading.Tasks;
using CQRSMagic.Commands;
using CQRSMagic.Events.Messaging;

namespace CQRSMagic
{
    public interface IMessageBus
    {
        Task<IEnumerable<IEvent>> SendCommandAsync(ICommand command);
    }
}