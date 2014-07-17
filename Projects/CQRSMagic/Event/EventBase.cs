using System;

namespace CQRSMagic.Event
{
    public abstract class EventBase : IEvent
    {
        protected EventBase()
        {
            // AzureEventSerializer requires a parameterless constructor.
        }

        protected EventBase(Guid aggregateId)
            : this(aggregateId, DateTime.Now)
        {
        }

        protected EventBase(Guid aggregateId, DateTimeOffset eventCreated)
        {
            AggregateId = aggregateId;
            EventCreated = eventCreated;
        }

        /// <summary>
        /// Gets or sets the aggregate Id.
        /// </summary>
        /// <remarks>
        /// AzureEventSerializer requires a set method to be protected.
        /// </remarks>
        public Guid AggregateId { get; protected set; }

        /// <summary>
        /// Gets or sets when the event created.
        /// </summary>
        /// <remarks>
        /// AzureEventSerializer requires a set method to be protected.
        /// </remarks>
        public DateTimeOffset EventCreated { get; protected set; }
    }
}