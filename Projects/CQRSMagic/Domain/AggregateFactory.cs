using System;

namespace CQRSMagic.Domain
{
    public class AggregateFactory : IAggregateFactory
    {
        public TAggregate CreateInstance<TAggregate>() where TAggregate : IAggregate
        {
            return Activator.CreateInstance<TAggregate>();
        }
    }
}