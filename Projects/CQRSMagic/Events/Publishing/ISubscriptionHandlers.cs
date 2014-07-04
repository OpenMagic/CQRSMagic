using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CQRSMagic.Events.Publishing
{
    public interface ISubscriptionHandlers
    {
        IEnumerable<Func<IEvent, Task>> FindSubscriptionsFor(IEvent @event);
    }
}