using System;

namespace CQRSMagic
{
    public abstract class AggregateCommandHandlers<TAggregate> : IAggregateCommandHandlers where TAggregate : IAggregate, new()
    {
        protected AggregateCommandHandlers()
        {
            AggregateType = typeof(TAggregate);
        }

        public Type AggregateType { get; private set; }
    }
}