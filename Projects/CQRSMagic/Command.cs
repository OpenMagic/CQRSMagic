using System;

namespace Library.CQRS
{
    public class Command : ICommand
    {
        public Command(Guid aggregateId)
        {
            AggregateId = aggregateId;
        }

        public Guid AggregateId { get; private set; }
    }
}