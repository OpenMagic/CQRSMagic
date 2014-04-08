using System.Collections.Generic;
using System.Threading.Tasks;
using CQRS.Example.CQRS.Events;

namespace CQRS.Example.CQRS.Commands
{
    /// <summary>
    ///     Represents the tasks that are created by <see cref="ICommandSender.SendCommand" />.
    /// </summary>
    public interface ISendCommandTasks
    {
        Task<IEnumerable<IEvent>> HandleCommand { get; }
        Task SaveEvents { get; }
        Task PublishEvents { get; }
        void WaitAll();
    }
}