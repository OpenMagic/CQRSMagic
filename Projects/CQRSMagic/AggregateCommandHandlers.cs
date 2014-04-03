using System;

namespace Library.CQRS
{
    public class AggregateCommandHandlers<TAggregate> : IAggregateCommandHandlers where TAggregate : IAggregate, new()
    {
        public AggregateCommandHandlers()
        {
            AggregateType = typeof(TAggregate);
        }

        public Type AggregateType { get; private set; }
    }
}