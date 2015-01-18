using System;

namespace CQRSMagic
{
    public interface IEventStore
    {
        void SaveEvents(Guid id, int version, IEvent[] events);
        TEntity GetEntity<TEntity>(Guid id) where TEntity : class, IEntity, new();
    }
}