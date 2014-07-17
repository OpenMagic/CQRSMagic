using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CQRSMagic.Event
{
    public interface IEventHandlers
    {
        void RegisterHandler<TEvent>(Func<TEvent, Task> handler);
        IEnumerable<Func<IEvent, Task>> GetEventHandlers(IEvent @event);
    }
}