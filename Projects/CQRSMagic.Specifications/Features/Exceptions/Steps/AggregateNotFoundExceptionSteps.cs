using System;
using CQRSMagic.Exceptions;
using CQRSMagic.Specifications.Support;
using TechTalk.SpecFlow;

namespace CQRSMagic.Specifications.Features.Exceptions.Steps
{
    [Binding]
    public class AggregateNotFoundExceptionSteps
    {
        private readonly ExceptionScenario ExceptionScenario;

        private Guid AggregateId;
        private Type AggregateType;

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