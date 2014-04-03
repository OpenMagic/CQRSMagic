using System;

namespace CQRSMagic.Exceptions
{
    public class EventsNotFoundException : Exception
    {
        public EventsNotFoundException(Type type, Guid aggregateId)
            : base(string.Format("Cannot find events for {0} aggregate with {1} id.", type.Name, aggregateId))
        {
        }
    }
}