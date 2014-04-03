using System;
using Library.CQRS.Exceptions;
using Library.CQRS.Specifications.Support;
using TechTalk.SpecFlow;

namespace Library.CQRS.Specifications.Features.Exceptions.Steps
{
    [Binding]
    public class EventsNotFoundExceptionSteps
    {
        private readonly ExceptionScenario ExceptionScenario;

        private Type AggregateType;
        private Guid AggregateId;
        
        public EventsNotFoundExceptionSteps(ExceptionScenario exceptionScenario)
        {
            ExceptionScenario = exceptionScenario;
        }

        [When(@"EventsNotFoundException is constructed with aggregate type '(.*)' and aggregate id is '(.*)'")]
        public void WhenEventsNotFoundExceptionIsConstructedWithAggregateTypeAndAggregateIdIs(string aggregateType, string aggregateId)
        {
            AggregateType = aggregateType.GetTypeFromName();
            AggregateId = new Guid(aggregateId);

            ExceptionScenario.Exception = new EventsNotFoundException(AggregateType, AggregateId);
        }
    }
}
