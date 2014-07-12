using CQRSMagic.Events.Publishing;
using ExampleDomain.Contacts;
using ExampleDomain.Contacts.Events;
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
                var subscriptionHandlerAssemblies = new[] {typeof(ContactSubscriptionHandlers).Assembly};

                // When
                var commandHandlers = SubscriptionHandlers.FindHandlers(subscriptionHandlerAssemblies);

                // Then
                commandHandlers.Keys.ShouldBeEquivalentTo(new[]
                {
                    typeof(AddedContact),
                    typeof(DeletedContact)
                });
            }
        }
    }
}