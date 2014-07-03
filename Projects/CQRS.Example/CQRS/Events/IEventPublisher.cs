using System.Collections.Generic;
using System.Threading.Tasks;

namespace CQRS.Example.CQRS.Events
{
    /// <summary>
    ///     Sends <see cref="IEvent">events</see> to their registered handler.
    /// </summary>
    public interface IEventPublisher
    {
        /// <summary>
        ///     Sends the events to registered handler.
        /// </summary>
        /// <param name="events">
        ///     The events to send.
        /// </param>
        IEnumerable<Task> PublishEvents(IEnumerable<IEvent> events);
    }
}