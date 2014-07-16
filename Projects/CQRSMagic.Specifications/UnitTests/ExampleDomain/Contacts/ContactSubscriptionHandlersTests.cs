using System;
using System.Linq;
using AzureMagic;
using ExampleDomain.Contacts;
using ExampleDomain.Contacts.Events;
using ExampleDomain.Contacts.Queries.Repositories;
using ExampleDomain.Repositories.Azure;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace CQRSMagic.Specifications.UnitTests.ExampleDomain.Contacts
{
    public class ContactSubscriptionHandlersTests : UnitTestsTestBase
    {
        public class HandleAddedContactEvent : ContactSubscriptionHandlersTests
        {
            [Fact]
            public void ShouldAddContactToRepository()
            {
                // Given
                var contactRepository = new AzureContactRepository(AzureStorage.DevelopmentConnectionString, ContactsTableName);
                var handlers = new ContactSubscriptionHandlers(contactRepository);
                var addedContact = new AddedContact();

                // When
                handlers.HandleEventAsync(addedContact).Wait();

                // Then
                contactRepository.FindAllContactsAsync().Result.Count().Should().Be(1);
            }
        }

        public class HandleDeletedContactEvent : ContactSubscriptionHandlersTests
        {
            [Fact]
            public void ShouldDeleteContactFromRepository()
            {
                // Given
                var contactRepository = A.Fake<IContactRepository>();
                var handlers = new ContactSubscriptionHandlers(contactRepository);
                var deletedContact = new DeletedContact(typeof(ContactAggregate), Guid.NewGuid());

                // When
                handlers.HandleEventAsync(deletedContact).Wait();

                // Then
                A.CallTo(() => contactRepository.DeleteContactByIdAsync(deletedContact.AggregateId))
                    .MustHaveHappened(Repeated.Exactly.Once);
            }
        }
    }
}
