using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CQRS.Example.CQRS.Events
{
    /// <summary>
    ///     Represents a collection of registered <see cref="IEventHandler{TEvent}">event handlers</see>.
    /// </summary>
    public interface IEventHandlers
    {
        /// <summary>
        ///     Gets the registered <see cref="IEventHandler{TEvent}">event handlers</see> for
        ///     <paramref name="eventType" />.
        /// </summary>
        /// <param name="eventType">
        ///     Type of the event to get registered <see cref="IEventHandler{TEvent}">event handlers</see> for.
        /// </param>
        IEnumerable<Func<IEvent, Task>> GetHandlers(Type eventType);

        /// <summary>
        ///     Registers a event handler.
        /// </summary>
        /// <typeparam name="TEventHandler">
        ///     The type the that implements event handler.
        /// </typeparam>
        /// <typeparam name="TEvent">
        ///     The type of event that is handled.
        /// </typeparam>
        void RegisterHandler<TEventHandler, TEvent>() where TEvent : class, IEvent;

        /// <summary>
        ///     Searches for, and registers, all event handlers in <paramref name="types" />.
        /// </summary>
        /// <param name="types">
        ///     The types to search for event handlers.
        /// </param>
        Task RegisterHandlers(IEnumerable<Type> types);
    }
}
