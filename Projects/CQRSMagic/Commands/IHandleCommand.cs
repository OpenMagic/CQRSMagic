using System.Collections.Generic;
using System.Threading.Tasks;
using CQRSMagic.Events.Messaging;

namespace CQRSMagic.Commands
{
    public interface IHandleCommand<in TCommand> where TCommand : ICommand
    {
        Task<IEnumerable<IEvent>> HandleCommandAsync(TCommand command);
    }
}