using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CQRS.Example.CQRS.Common;
using CQRS.Example.CQRS.Events;

namespace CQRS.Example.CQRS.Commands
{
    /// <summary>
    ///     Represents a collection of registered <see cref="ICommandHandler{TCommand}">command handlers</see>.
    /// </summary>
    public interface ICommandHandlers : IMessageHandlers<ICommand>
    {
        /// <summary>
        ///     Gets the registered <see cref="ICommandHandler{TCommand}">command handler</see> for
        ///     <paramref name="commandType" />.
        /// </summary>
        /// <param name="commandType">
        ///     Type of the command to get registered <see cref="ICommandHandler{TCommand}">command handler</see> for.
        /// </param>
        Func<ICommand, Task<IEnumerable<IEvent>>> GetHandler(Type commandType);
    }
}