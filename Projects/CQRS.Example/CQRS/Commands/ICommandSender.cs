using System.Collections.Generic;
using System.Threading.Tasks;
using CQRS.Example.CQRS.Events;

namespace CQRS.Example.CQRS.Commands
{
    /// <summary>
    ///     Sends <see cref="ICommand">commands</see> to their registered handler.
    /// </summary>
    public interface ICommandSender
    {
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