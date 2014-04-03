using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.CQRS
{
    public interface ICommandHandler
    {
        Type AggregateType { get; }
        Type CommandType { get; }

        Task<IEnumerable<IEvent>> SendCommand(ICommand command, IEventStore eventStore);
    }
}