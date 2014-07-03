using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Example.CQRS.Common;
using Microsoft.Practices.ServiceLocation;

namespace CQRS.Example.CQRS.Events
{
    public class EventHandlers : MessageHandlersBase<IEventHandler<IEvent>, IEvent, Task>, IEventHandlers
    {
        public EventHandlers(IServiceLocator container)
            : base(container)
        {
        }

        public IEnumerable<Func<IEvent, Task>> GetHandlers(Type eventType)
        {
            return
                from keyValuePair in Handlers
                where keyValuePair.Key == eventType
                select keyValuePair.Value;
        }
    }
}