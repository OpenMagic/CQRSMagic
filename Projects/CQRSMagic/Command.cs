using System;

namespace CQRSMagic
{
    public abstract class Command : ICommand
    {
        protected Command(Guid aggregateId)
        {
            AggregateId = aggregateId;
        }

        public Guid AggregateId { get; private set; }
    }
}