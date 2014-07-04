using System.Collections.Generic;

namespace CQRSMagic.Events
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