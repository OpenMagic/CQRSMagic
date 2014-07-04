using System.Collections.Generic;
using CQRSMagic.Events.Sourcing;

namespace CQRSMagic.Events.Messaging
{
    public class EventBus : IEventBus
    {
        private readonly IEventStore EventStore;

        public EventBus(IEventStore eventStore)
        {
            // todo: unit tests
            EventStore = eventStore;
        }

        public void SendEvents(IEnumerable<IEvent> events)
        {
            // todo: unit tests
            EventStore.SaveEvents(events);
        }
    }
}