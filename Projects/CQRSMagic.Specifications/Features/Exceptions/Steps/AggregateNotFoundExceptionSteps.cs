using System;
using Library.CQRS.Exceptions;
using Library.CQRS.Specifications.Support;
using TechTalk.SpecFlow;

namespace Library.CQRS.Specifications.Features.Exceptions.Steps
{
    [Binding]
    public class AggregateNotFoundExceptionSteps
    {
        private readonly ExceptionScenario ExceptionScenario;

        private Type AggregateType;
        private Guid AggregateId;

        public AggregateNotFoundExceptionSteps(ExceptionScenario exceptionScenario)
        {
            ExceptionScenario = exceptionScenario;
        }

        [When(@"AggregateNotFoundException is constructed with aggregate type '(.*)' and aggregate id is '(.*)'")]
        public void WhenAggregateNotFoundExceptionIsConstructedWithAggregateTypeAndAggregateIdIs(string aggregateType, string aggregateId)
        {
            AggregateType = aggregateType.GetTypeFromName();
            AggregateId = new Guid(aggregateId);

            ExceptionScenario.Exception = new AggregateNotFoundException(AggregateType, AggregateId);
        }
    }
}
