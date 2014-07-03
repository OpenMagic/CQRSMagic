using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CQRS.Example.CQRS.Common;

namespace CQRS.Example.CQRS.Events
{
    /// <summary>
    ///     Represents a collection of registered <see cref="IEventHandler{TEvent}">event handlers</see>.
    /// </summary>
    public interface IEventHandlers : IMessageHandlers<IEvent>
    {
        /// <summary>
        ///     Gets the registered <see cref="IEventHandler{TEvent}">event handlers</see> for
        ///     <paramref name="eventType" />.
        /// </summary>
        /// <param name="eventType">
        ///     Type of the event to get registered <see cref="IEventHandler{TEvent}">event handlers</see> for.
        /// </param>
        IEnumerable<Func<IEvent, Task>> GetHandlers(Type eventType);

    }
}
