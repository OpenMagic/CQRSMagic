using System;

namespace CQRSMagic
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