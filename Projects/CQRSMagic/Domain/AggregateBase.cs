using System;
using System.Collections.Generic;
using System.Linq;
using CQRSMagic.Event;

namespace CQRSMagic.Domain
{
    public abstract class AggregateBase : IAggregate
    {
        private static readonly AggregateEventHandlers EventHandlers = new AggregateEventHandlers();

        public Guid Id { get; protected set; }

        public void ApplyEvents(IEnumerable<IEvent> events)
        {
            foreach (var @event in events.OrderBy(e => e.EventCreated))
            {
                ApplyEvent(@event);
            }
        }

        private void ApplyEvent(IEvent @event)
        {
            try
            {
                var eventHandler = EventHandlers.FindEventHandler(GetType(), @event);

                if (eventHandler == null)
                {
                    return;
                }

                try
                {
                    eventHandler.Invoke(this, new object[] {@event});
                }
                catch (Exception exception)
                {
                    var message = string.Format("Cannot apply {0} event to {1}/{2} with {3}.{4}.", @event.GetType(), GetType(), @event.AggregateId, eventHandler.DeclaringType, eventHandler.Name);
                    throw new EventException(message, exception);
                }
            }
            catch (Exception exception)
            {
                var message = string.Format("Cannot apply {0} event to {1}/{2}.", @event.GetType(), GetType(), @event.AggregateId);
                throw new EventException(message, exception);
            }
        }
    }
}