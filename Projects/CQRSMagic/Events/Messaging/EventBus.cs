using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task SendEventsAsync(IEnumerable<IEvent> events)
        {
            // todo: unit tests
            await EventStore.SaveEventsAsync(events);
        }
    }
}