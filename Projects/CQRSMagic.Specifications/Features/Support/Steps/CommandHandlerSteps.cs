using System;
using CQRSMagic.Specifications.Support.Fakes;
using CQRSMagic.Support;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace CQRSMagic.Specifications.Features.Support.Steps
{
    [Binding]
    public class CommandHandlerSteps
    {
        private readonly IAggregateCommandHandlers AggregateCommandHandlers;
        private readonly Type CommandInterface;
        private readonly Type AggregateType;
        private readonly Type CommandType;
        private CommandHandler CommandHandler;

        public CommandHandlerSteps()
        {
            AggregateCommandHandlers = new FakeAggregateCommandHandlers();
            CommandInterface = typeof(IHandleCommand<FakeCommand>);
            CommandType = typeof(FakeCommand);
            AggregateType = typeof(FakeAggregate);
        }

        [When(@"I create a new CommandHandler with valid parameters")]
        public void WhenICreateANewCommandHandlerWithValidParameters()
        {
            CommandHandler = new CommandHandler(AggregateCommandHandlers, CommandInterface, CommandType, AggregateType);
        }

        [Then(@"AggregateType is set")]
        public void ThenAggregateTypeIsSet()
        {
            CommandHandler.AggregateType.Should().Be(AggregateType);
        }

        [Then(@"CommandType is set")]
        public void ThenCommandTypeIsSet()
        {
            CommandHandler.CommandType.Should().Be(CommandType);
        }

        [Then(@"SendCommand works")]
        public void ThenSendCommandWorks()
        {
            var sendCommandTask = CommandHandler.SendCommand(new FakeCommand(), new FakeEventStore());
            
            sendCommandTask.Result.Should().NotBeEmpty();
        }
    }
}
