using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Library.CQRS.Exceptions;
using Library.CQRS.Specifications.Support.Fakes;
using TechTalk.SpecFlow;

namespace Library.CQRS.Specifications.Features.Steps
{
    [Binding]
    public class EventStoreSteps
    {
        private readonly IEventStore EventStore;
        private readonly FakeEventStoreRepository Repository;

        private Guid AggregateId;
        private AggregateNotFoundException AggregateNotFoundException;
        private IList<SimpleFakeEvent> ExistingEvents;
        private IList<SimpleFakeEvent> NewEvents;
        private SimpleFakeAggregate SimpleFakeAggregate;
        private EventsNotFoundException EventsNotFoundException;
        private IEvent[] RetrievedEvents;

        public EventStoreSteps(IEventStore eventStore, FakeEventStoreRepository repository)
        {
            EventStore = eventStore;
            Repository = repository;

            ExistingEvents = new List<SimpleFakeEvent>();
            NewEvents = new List<SimpleFakeEvent>();
        }

        [Given(@"a new aggregate")]
        public void GivenANewAggregate()
        {
            AggregateId = Guid.NewGuid();
        }

        [Given(@"an existing aggregate")]
        public void GivenAnExistingAggregate()
        {
            AggregateId = Guid.NewGuid();
        }

        [Given(@"there are existing events")]
        public void GivenThereAreExistingEvents()
        {
            ExistingEvents = CreateFakeEvents(2, 1);
            SaveEvents(AggregateId, ExistingEvents);
        }

        [Given(@"there are new events")]
        public void GivenThereAreNewEvents()
        {
            NewEvents = CreateFakeEvents(2, ExistingEvents.Count + 1);
        }

        [When(@"SaveEventForEvents is called")]
        public void WhenSaveEventForEventsIsCalled()
        {
            SaveEvents(AggregateId, NewEvents);
        }

        private void SaveEvents(Guid aggregateId, IEnumerable<SimpleFakeEvent> events)
        {
            EventStore.SaveEventsFor(typeof(SimpleFakeAggregate), aggregateId, events).Wait();
        }

        [Then(@"the events are saved to the database")]
        public void ThenTheEventsAreSavedToTheDatabase()
        {
            var rows = Repository.GetRows().ToArray();

            rows.Count().Should().Be(1);

            rows[0].AggregateType.Should().Be(typeof(SimpleFakeAggregate));
            rows[0].Events.ShouldBeEquivalentTo(ExistingEvents.Concat(NewEvents));
        }

        [Given(@"aggregate does not exist")]
        public void GivenAggregateDoesNotExist()
        {
            AggregateId = Guid.NewGuid();
        }

        [When(@"GetAggregate is called")]
        public void WhenGetAggregateIsCalled()
        {
            try
            {
                SimpleFakeAggregate = EventStore.GetAggregate<SimpleFakeAggregate>(AggregateId).Result;
            }
            catch (AggregateException ex)
            {
                AggregateNotFoundException = (AggregateNotFoundException) ex.InnerExceptions.Single();
            }
        }

        [Then(@"AggregateNotFoundException is thrown")]
        public void ThenAggregateNotFoundExceptionIsThrown()
        {
            SimpleFakeAggregate.Should().BeNull();
            AggregateNotFoundException.Should().NotBeNull();
        }

        [Then(@"the aggregate is found")]
        public void ThenTheAggregateIsFound()
        {
            SimpleFakeAggregate.Should().NotBeNull();
        }

        private static IList<SimpleFakeEvent> CreateFakeEvents(int count, int startingId)
        {
            var events = new List<SimpleFakeEvent>();

            for (var i = 0; i < count; i++)
            {
                events.Add(new SimpleFakeEvent(i + startingId));
            }

            return events;
        }

        [Then(@"the events are sent to the aggregate")]
        public void ThenTheEventsAreSentToTheAggregate()
        {
            SimpleFakeAggregate.HandledEvents.ShouldAllBeEquivalentTo(ExistingEvents.Concat(NewEvents));
        }

        [When(@"GetEventsFor is called")]
        public void WhenGetEventsForIsCalled()
        {
            try
            {
                RetrievedEvents = EventStore.GetEventsFor<SimpleFakeAggregate>(AggregateId).Result.ToArray();
            }
            catch (AggregateException ex)
            {
                EventsNotFoundException = (EventsNotFoundException) ex.InnerExceptions.Single();
            }
        }

        [Then(@"EventsNotFoundException is thrown")]
        public void ThenEventsNotFoundExceptionIsThrown()
        {
            RetrievedEvents.Should().BeNull();
            EventsNotFoundException.Should().NotBeNull();
        }

        [Then(@"the events are found")]
        public void ThenTheEventsAreFound()
        {
            RetrievedEvents.ShouldAllBeEquivalentTo(ExistingEvents.Concat(NewEvents));
        }
    }
}