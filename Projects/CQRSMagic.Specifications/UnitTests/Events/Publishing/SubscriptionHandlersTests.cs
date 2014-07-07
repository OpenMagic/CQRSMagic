using CQRSMagic.Events.Publishing;
using ExampleDomain.Contacts.Events;
using ExampleDomain.Contacts.Queries;
using FluentAssertions;
using Xunit;

namespace CQRSMagic.Specifications.UnitTests.Events.Publishing
{
    public class SubscriptionHandlersTests
    {
        public class FindHandlers
        {
            [Fact]
            public void ShouldReturnListOfCommandHandlersInDomainAssemblies()
            {
                // Given
                var subscriptionHandlerAssemblies = new[] {typeof(ContactQuery).Assembly};

                // When
                var commandHandlers = SubscriptionHandlers.FindHandlers(subscriptionHandlerAssemblies);

                // Then
                commandHandlers.Keys.ShouldBeEquivalentTo(new[]
                {
                    typeof(ContactAdded)
                });
            }
        }
    }
}