using System;
using OpenMagic.Exceptions;

namespace CQRSMagic
{
    public class EventStore : IEventStore
    {
        public TAggregate GetAggregate<TAggregate>(Guid aggregateId) where TAggregate : IAggregate
        {
            throw new ToDoException();
        }
    }
}