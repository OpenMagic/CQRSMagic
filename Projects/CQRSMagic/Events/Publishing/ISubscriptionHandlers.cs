using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CQRSMagic.Events.Messaging;

namespace CQRSMagic.Events.Publishing
{
    public interface ISubscriptionHandlers
    {
        IEnumerable<Func<IEvent, Task>> FindSubscriptionsFor(IEvent @event);
    }
}