using System.Collections.Generic;
using System.Threading.Tasks;
using CQRS.Example.CQRS.Events;

namespace CQRS.Example.CQRS.EventStore
{
    public class EventStore : IEventStore
    {
        private readonly IEventRepository Repository;

        public EventStore(IEventRepository repository)
        {
            Repository = repository;
        }

        public Task SaveEvents(IEnumerable<IEvent> events)
        {
            return Repository.SaveEvents(events);
        }
    }
}