using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CQRSMagic.Event;

namespace CQRSMagic.Command
{
    public interface ICommandBus
    {
        Task SendCommandAsync(ICommand command);
        void RegisterHandler<TCommand>(Func<TCommand, Task<IEnumerable<IEvent>>> handler) where TCommand : ICommand;
    }
}