using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CQRS.Example.CQRS.Events;

namespace CQRS.Example.CQRS.Commands
{
    /// <summary>
    ///     Represents a collection of registered <see cref="ICommandHandler{TCommand}">command handlers</see>.
    /// </summary>
    public interface ICommandHandlers
    {
        /// <summary>
        ///     Gets the registered <see cref="ICommandHandler{TCommand}">command handler</see> for
        ///     <paramref name="commandType" />.
        /// </summary>
        /// <param name="commandType">
        ///     Type of the command to get registered <see cref="ICommandHandler{TCommand}">command handler</see> for.
        /// </param>
        Func<ICommand, Task<IEnumerable<IEvent>>> GetHandler(Type commandType);

        /// <summary>
        ///     Registers a command handler.
        /// </summary>
        /// <typeparam name="TCommandHandler">
        ///     The type the that implements command handler.
        /// </typeparam>
        /// <typeparam name="TCommand">
        ///     The type of command that is handled.
        /// </typeparam>
        void RegisterHandler<TCommandHandler, TCommand>() where TCommand : class, ICommand;

        /// <summary>
        ///     Searches for, and registers, all command handlers in <paramref name="types" />.
        /// </summary>
        /// <param name="types">
        ///     The types to search for command handlers.
        /// </param>
        Task RegisterHandlers(IEnumerable<Type> types);
    }
}