using System;
using CQRSMagic.Domain;
using OpenMagic.Exceptions;

namespace CQRSMagic.Exceptions
{
    public class AggregateNotFoundException<TAggregate> : Exception where TAggregate : IAggregate
    {
        public AggregateNotFoundException(Guid aggregateId)
        {
            throw new ToDoException();
        }
    }
}