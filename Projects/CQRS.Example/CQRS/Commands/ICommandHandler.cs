using System.Collections.Generic;
using System.Threading.Tasks;
using CQRS.Example.CQRS.Common;
using CQRS.Example.CQRS.Events;

namespace CQRS.Example.CQRS.Commands
{
    public interface ICommandHandler<in TCommand> : IMessageHandler<TCommand> where TCommand : ICommand
    {
        Task<IEnumerable<IEvent>> Handle(TCommand command);
    }
}