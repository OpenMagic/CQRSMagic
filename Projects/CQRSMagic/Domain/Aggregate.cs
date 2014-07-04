using System;
using System.Collections.Generic;
using CQRSMagic.Domain.Support;
using CQRSMagic.Events;

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

        public Guid Id { get; private set; }

        public void ApplyEvents(IEnumerable<IEvent> events)
        {
            // todo: unit tests
            foreach (var @event in events)
            {
                ApplyEvent(@event);
            }
        }

        private void ApplyEvent(IEvent @event)
        {
            var eventHandler = EventHandlers.FindEventHandler(@event);

            if (eventHandler == null)
            {
                return;
            }

            eventHandler.Invoke(this, new object[] {@event});
        }
    }
}