using System.Collections.Generic;
using System.Threading.Tasks;
using CQRSMagic.Events.Messaging;

namespace CQRSMagic.Commands
{
    public interface ICommandBus
    {
        Task<IEnumerable<IEvent>> SendCommandAsync(ICommand command);
    }
}