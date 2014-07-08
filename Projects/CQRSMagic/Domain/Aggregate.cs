using System;
using System.Collections.Generic;
using System.Linq;
using Anotar.CommonLogging;
using CQRSMagic.Domain.Support;
using CQRSMagic.Events.Messaging;

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
            ApplyEvents(events.ToArray());
        }

        private void ApplyEvents(IEvent[] events)
        {
            LogTo.Debug("ApplyEvents(IEvent[{0}] events)", events.Length);

            // todo: unit tests
            foreach (var @event in events)
            {
                ApplyEvent(@event);
            }

            LogTo.Debug("Exit - ApplyEvents(IEvent[{0}] events)", events.Length);
        }

        private void ApplyEvent(IEvent @event)
        {
            LogTo.Debug("ApplyEvent(IEvent)");

            var eventHandler = EventHandlers.FindEventHandler(@event);

            if (eventHandler == null)
            {
                LogTo.Debug("No event handler");
                LogTo.Debug("Exit - ApplyEvent(IEvent)");
                return;
            }

            eventHandler.Invoke(this, new object[] {@event});
            LogTo.Debug("Exit - ApplyEvent(IEvent)");
        }
    }
}