using System.Collections.Generic;
using System.Threading.Tasks;
using CQRS.Example.CQRS.Events;

namespace CQRS.Example.CQRS.Persistence
{
    public class EventRepository : IEventRepository
    {
        private readonly IList<IEvent> Events = new List<IEvent>();

        public Task SaveEvents(IEnumerable<IEvent> events)
        {
            return Task.Factory.StartNew(() =>
            {
                foreach (var @event in events)
                {
                    Events.Add(@event);
                }
            });
        }
    }
}