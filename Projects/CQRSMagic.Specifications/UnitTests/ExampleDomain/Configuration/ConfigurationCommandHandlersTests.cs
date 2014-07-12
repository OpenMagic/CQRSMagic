using System;
using System.Linq;
using CQRSMagic.Specifications.Support;
using ExampleDomain.Configuration;
using ExampleDomain.Configuration.Commands;
using ExampleDomain.Contacts;
using ExampleDomain.Repositories.InMemory;
using FluentAssertions;
using OpenMagic.Exceptions;
using Xunit;

namespace CQRSMagic.Specifications.UnitTests.ExampleDomain.Configuration
{
    public class ConfigurationCommandHandlersTests : ExampleDomainTestBase
    {
        public class ClearCommand : ConfigurationCommandHandlersTests
        {
            [Fact]
            public void ShouldReturn_DeletedContactEvent_ForEachContactInTheRepository()
            {
                // Given
                var contactRepository = new InMemoryContactRepository();
                var commandHandlers = new ConfigurationCommandHandlers(contactRepository);
                var contacts = FakeModels.Contacts(3).ToArray();

                contactRepository.AddContacts(contacts);

                var expectedMinimumEventCreated = DateTime.UtcNow;

                // When
                var events = commandHandlers.HandleCommandAsync(new ClearAll()).Result.ToArray();

                // Then
                events.Length.Should().Be(contacts.Length);
                events.Select(e => e.AggregateId).Should().BeEquivalentTo(contacts.Select(c => c.Id));

                foreach (var @event in events)
                {
                    @event.AggregateType.Should().Be(typeof(ContactAggregate));
                    @event.EventCreated.Ticks.Should().BeGreaterOrEqualTo(expectedMinimumEventCreated.Ticks).And.BeLessThan(expectedMinimumEventCreated.Ticks + TimeSpan.FromMilliseconds(100).Ticks);
                }
            }
        }
    }
}
