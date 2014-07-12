using System;
using ExampleDomain.Contacts;
using ExampleDomain.Contacts.Events;
using ExampleDomain.Contacts.Queries.Repositories;
using FakeItEasy;
using Xunit;

namespace CQRSMagic.Specifications.UnitTests.ExampleDomain.Contacts
{
    public class ContactSubscriptionHandlersTests : UnitTestsTestBase
    {
        // ReSharper disable once InconsistentNaming
        public class Handle_DeletedContact_Event : ContactSubscriptionHandlersTests
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
