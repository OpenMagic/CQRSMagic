using CQRSMagic.Commands;
using ExampleDomain.Contacts.Commands;
using FluentAssertions;
using OpenMagic.Exceptions;
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
                commandHandlers.Keys.ShouldBeEquivalentTo(new []
                {
                    typeof(AddContact)
                });
            }

            [Fact(Skip = "todo")]
            public void ShouldThrow_DuplicateCommandHandlerException_WhenMultipleCommandHandlersAreFoundForTheSameCommand()
            {
            }
        }
    }
}
