using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CQRSMagic
{
    public interface ICommandHandler
    {
        Type AggregateType { get; }
        Type CommandType { get; }

        Task<IEnumerable<IEvent>> SendCommand(ICommand command, IEventStore eventStore);
    }
}