using System.Collections.Generic;
using System.Threading.Tasks;
using CQRSMagic.Event;

namespace CQRSMagic.Command
{
    public interface IHandleCommand<in TCommand> where TCommand : ICommand
    {
        Task<IEnumerable<IEvent>> HandleCommand(TCommand command);
    }
}