using CQRSMagic.Commands;
using ExampleDomain.Configuration.Commands;
using ExampleDomain.Contacts.Commands;
using FluentAssertions;
using Xunit;

namespace CQRSMagic.Specifications.UnitTests.Commands
{
    public class CommandHandlersTests
    {
        public class FindHandlers
        {
            [Fact]
            public void ShouldReturnListOfCommandHandlersInDomainAssemblies()
            {
                // Given
                var commandHandlerAssemblies = new[] {typeof(AddContact).Assembly};

                // When
                var commandHandlers = CommandHandlers.FindHandlers(commandHandlerAssemblies);

                // Then
                commandHandlers.Keys.ShouldBeEquivalentTo(new[]
                {
                    typeof(AddContact),
                    typeof(ClearAll)
                });
            }

            [Fact(Skip = "todo")]
            public void ShouldThrow_DuplicateCommandHandlerException_WhenMultipleCommandHandlersAreFoundForTheSameCommand()
            {
            }
        }
    }
}