using System;
using CQRSMagic.Specifications.Support.Fakes;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace CQRSMagic.Specifications.Features.Steps
{
    [Binding]
    public class CommandSteps
    {
        private Guid AggregateId;
        private FakeCommand Command;

        [Given(@"AggregateId is ""(.*)""")]
        public void GivenAggregateIdIs(string aggregateId)
        {
            AggregateId = new Guid(aggregateId);
        }

        [When(@"I create a new Command")]
        public void WhenICreateANewCommand()
        {
            Command = new FakeCommand(AggregateId);
        }

        [Then(@"Command\.AggregateId should equal AggregateId")]
        public void ThenCommand_AggregateIdShouldEqualAggregateId()
        {
            Command.AggregateId.Should().Be(AggregateId);
        }
    }
}
