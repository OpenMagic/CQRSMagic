using CQRSMagic.Specifications.Support.Fakes;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace CQRSMagic.Specifications.Features.Steps
{
    [Binding]
    public class AggregateCommandHandlersSteps
    {
        private IAggregateCommandHandlers CommandHandlers;

        [When(@"I create new AggregateCommandHandlers")]
        public void WhenICreateNewAggregateCommandHandlers()
        {
            CommandHandlers = new FakeAggregateCommandHandlers();
        }

        [Then(@"AggregateType equals typeof\(TAggregate\)")]
        public void ThenAggregateTypeEqualsTypeofTAggregate()
        {
            CommandHandlers.AggregateType.Should().Be(typeof(FakeAggregate));
        }
    }
}
