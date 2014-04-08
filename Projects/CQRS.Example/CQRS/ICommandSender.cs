using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CQRS.Example.CQRS.Commands;
using CQRS.Example.CQRS.Events;

namespace CQRS.Example.CQRS
{
    public interface ICommandSender
    {
        void RegisterHandler<TCommandHandler, TCommand>() where TCommand : class, ICommand;

        Task RegisterHandlers(IEnumerable<Type> types);

        Task<IEnumerable<IEvent>> SendCommand(ICommand command);
    }
}