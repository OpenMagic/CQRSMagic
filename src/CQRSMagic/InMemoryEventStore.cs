using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace CQRSMagic
{
    public class InMemoryEventStore : IEventStore
    {
        private static readonly ConcurrentDictionary<Guid, IList<IEvent>> Entities = new ConcurrentDictionary<Guid, IList<IEvent>>();

        public void SaveEvents(Guid id, int version, IEvent[] newEvents)
        {
            var currentEvents = Entities.GetOrAdd(id, key => new List<IEvent>());

            if (version != currentEvents.Count)
            {
                throw new ConcurrencyException(id, version, currentEvents.Count);
            }

            foreach (var newEvent in newEvents)
            {
                currentEvents.Add(newEvent);
            }
        }

        public TEntity GetEntity<TEntity>(Guid id) where TEntity : class, IEntity, new()
        {
            IList<IEvent> events;

            if (!Entities.TryGetValue(id, out events))
            {
                throw new EntityNotFoundException<TEntity>(id);
            }

            var entity = Activator.CreateInstance<TEntity>();

            entity.ApplyEvents(events);

            return entity;
        }
    }
}
