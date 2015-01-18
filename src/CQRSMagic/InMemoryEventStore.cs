using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace CQRSMagic
{
    public class InMemoryEventStore : IEventStore
    {
        private readonly ConcurrentDictionary<Guid, IList<IEvent>> _entities = new ConcurrentDictionary<Guid, IList<IEvent>>();

        public void SaveEvents(Guid id, int version, IEvent[] newEvents)
        {
            var currentEvents = _entities.GetOrAdd(id, key => new List<IEvent>());

            if (version != currentEvents.Count)
            {
                throw new ConcurrencyException(id, version, currentEvents.Count);
            }

            foreach (var newEvent in newEvents)
            {
                currentEvents.Add(newEvent);
            }
        }
    }
}
