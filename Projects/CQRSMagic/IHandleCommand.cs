using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.CQRS
{
    public interface IHandleCommand<in TCommand> where TCommand : ICommand
    {
        Task<IEnumerable<IEvent>> HandleCommand(TCommand command, IEventStore eventStore);
    }
}