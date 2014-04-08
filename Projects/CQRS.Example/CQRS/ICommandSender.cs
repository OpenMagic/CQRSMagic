using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CQRS.Example.CQRS.Commands;
using CQRS.Example.CQRS.Events;

namespace CQRS.Example.CQRS
{
    /// <summary>
    /// todo: document after splitting ICommandSender and ICommandHandlers.
    /// </summary>
    public interface ICommandSender
    {
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
        ///     Searches for, and registers, all command handles in <paramref name="types" />.
        /// </summary>
        /// <param name="types">
        ///     The types to search for command handlers.
        /// </param>
        /// <returns>
        ///     A task.
        /// </returns>
        Task RegisterHandlers(IEnumerable<Type> types);

        /// <summary>
        ///     Sends the command to the registered handler.
        /// </summary>
        /// <param name="command">
        ///     The command to send.
        /// </param>
        /// <returns>
        ///     List of events raised by the command.
        /// </returns>
        Task<IEnumerable<IEvent>> SendCommand(ICommand command);
    }
}