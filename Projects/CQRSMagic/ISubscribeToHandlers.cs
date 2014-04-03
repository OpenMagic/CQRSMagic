using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.CQRS
{
    public interface ISubscribeToHandlers
    {
        Task PublishEvents(Guid aggregateId, IEnumerable<IEvent> events);
    }
}