using System.Collections.Generic;
using System.Threading.Tasks;
using CQRS.Example.CQRS.Commands;
using CQRS.Example.CQRS.Events;

namespace CQRS.Example.CQRS
{
    public interface ICommandSender
    {
        Task<IEnumerable<IEvent>> SendCommand(ICommand command);
        void RegisterHandler<TCommandHandler, TCommand>() where TCommand : class, ICommand;
    }
}