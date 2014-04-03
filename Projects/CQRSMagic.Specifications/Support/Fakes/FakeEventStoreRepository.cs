using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenMagic.Extensions.Collections.Generic;

namespace Library.CQRS.Specifications.Support.Fakes
{
    public class FakeEventStoreRepository : IEventStoreRepository
    {
        private readonly IList<Row> Rows = new List<Row>();

        public async Task<IEnumerable<IEvent>> FindEventsFor(Type aggregateType, Guid aggregateId)
        {
            var row = await FindRow(aggregateType, aggregateId);

            return row != null ? row.Events.AsEnumerable() : null;
        }

        public async Task SaveEventsFor(Type aggregateType, Guid aggregateId, IEnumerable<IEvent> events)
        {
            var row = await FindRow(aggregateType, aggregateId);

            if (row == null)
            {
                Rows.Add(new Row(aggregateType, aggregateId, events));
            }
            else
            {
                row.AddEvents(events);
            }
        }

        internal IEnumerable<Row> GetRows()
        {
            return Rows.AsEnumerable();
        }

        private Task<Row> FindRow(Type aggregateType, Guid aggregateId)
        {
            return Task.Factory.StartNew(() => Rows.SingleOrDefault(row => row.AggregateType == aggregateType && row.AggregateId == aggregateId));
        }

        internal class Row
        {
            public readonly Guid AggregateId;
            public readonly Type AggregateType;
            public readonly IList<IEvent> Events;

            public Row(Type aggregateType, Guid aggregateId, IEnumerable<IEvent> events)
            {
                AggregateType = aggregateType;
                AggregateId = aggregateId;
                Events = new List<IEvent>(events);
            }

            public void AddEvents(IEnumerable<IEvent> events)
            {
                events.ForEach(e => Events.Add(e));
            }
        }

    }
}