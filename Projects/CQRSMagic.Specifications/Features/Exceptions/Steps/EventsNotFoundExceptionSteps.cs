using System;
using CQRSMagic.Exceptions;
using CQRSMagic.Specifications.Support;
using TechTalk.SpecFlow;

namespace CQRSMagic.Specifications.Features.Exceptions.Steps
{
    [Binding]
    public class EventsNotFoundExceptionSteps
    {
        private readonly ExceptionScenario ExceptionScenario;

        private Guid AggregateId;
        private Type AggregateType;

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