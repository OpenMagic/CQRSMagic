using FluentAssertions;
using TechTalk.SpecFlow;

namespace CQRSMagic.Specifications.Features.Exceptions.Steps
{
    [Binding]
    public class ExceptionSteps
    {
        private readonly ExceptionScenario ExceptionScenario;

        public ExceptionSteps(ExceptionScenario exceptionScenario)
        {
            ExceptionScenario = exceptionScenario;
        }

        [Then(@"the exception message should be '(.*)'")]
        public void ThenTheMessageShouldBe(string expectedMessage)
        {
            ExceptionScenario.Exception.Message.Should().Be(expectedMessage);
        }
    }
}