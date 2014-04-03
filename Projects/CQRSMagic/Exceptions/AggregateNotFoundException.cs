using System;

namespace CQRSMagic.Exceptions
{
    public class AggregateNotFoundException : Exception
    {
        public AggregateNotFoundException(Type type, Guid aggregateId)
            : base(string.Format("Cannot find {0} aggregate with {1} id.", type.Name, aggregateId))
        {
        }
    }
}