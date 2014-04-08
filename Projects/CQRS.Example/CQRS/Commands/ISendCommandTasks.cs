using System.Collections.Generic;
using System.Threading.Tasks;
using CQRS.Example.CQRS.Events;

namespace CQRS.Example.CQRS.Commands
{
    public interface ISendCommandTasks
    {
        Task<IEnumerable<IEvent>> HandleCommand { get; }
        Task SaveEvents { get; }
        Task PublishEvents { get; }
        void WaitAll();
    }
}