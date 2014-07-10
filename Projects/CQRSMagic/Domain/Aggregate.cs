using System;
using System.Collections.Generic;
using System.Linq;
using CQRSMagic.Domain.Support;
using CQRSMagic.Events.Messaging;
using CQRSMagic.Exceptions;

namespace CQRSMagic.Domain
{
    public class Aggregate : IAggregate
    {
        private readonly EventHandlers EventHandlers;

        public Aggregate()
        {
            // todo: unit tests
            EventHandlers = new EventHandlers(GetType());
        }

        public Guid Id { get; protected set; }

        public void ApplyEvents(IEnumerable<IEvent> events)
        {
            ApplyEvents(events.ToArray());
        }

        private void ApplyEvents(IEvent[] events)
        {
            // todo: unit tests
            foreach (var @event in events)
            {
                ApplyEvent(@event);
            }
        }

        private void ApplyEvent(IEvent @event)
        {
            try
            {
                var eventHandler = EventHandlers.FindEventHandler(@event);

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