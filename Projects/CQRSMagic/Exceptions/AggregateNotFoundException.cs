using System;
using CQRSMagic.Domain;

namespace CQRSMagic.Exceptions
{
    public class AggregateNotFoundException<TAggregate> : CQRSMagicException where TAggregate : IAggregate
    {
        public AggregateNotFoundException(Guid aggregateId)
            : base(CreateMessage(aggregateId))
        {
        }

        private static string CreateMessage(Guid aggregateId)
        {
            // todo: unit tests
            return string.Format("Cannot find {0} aggregate with {1} id.", typeof(TAggregate), aggregateId);
        }
    }
}