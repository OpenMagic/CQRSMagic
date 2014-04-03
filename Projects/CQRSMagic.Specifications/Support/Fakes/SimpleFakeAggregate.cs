using System;
using System.Collections.Generic;

namespace Library.CQRS.Specifications.Support.Fakes
{
    internal class SimpleFakeAggregate : Aggregate, IHandleEvent<SimpleFakeEvent>
    {
        internal readonly IList<IEvent> HandledEvents = new List<IEvent>();

        public string StringValue { get; private set; }
        public int IntValue { get; private set; }
        public DateTime DateValue { get; private set; }

        public void HandleEvent(SimpleFakeEvent e)
        {
            HandledEvents.Add(e);
        }
    }
}
