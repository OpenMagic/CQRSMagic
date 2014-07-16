using System;
using System.Linq;
using System.Threading.Tasks;
using CQRSMagic.Specifications.Support;
using ExampleDomain.Repositories;
using FakeItEasy.ExtensionSyntax.Full;
using FluentAssertions;
using Ninject;
using Xunit;

namespace CQRSMagic.Specifications.UnitTests.ExampleDomain.Repositories
{
    public abstract class ContactRepositoryTestBase : UnitTestsTestBase
    {
        protected ContactRepositoryTestBase(Func<IKernel, IRepositoryFactory> createRepositoryFactory)
            : base(createRepositoryFactory)
        {
        }

        [Fact]
        public async Task DeleteContactByIdAsync_ShouldDeleteContactFromRepository()
        {
            // Given
            var contact = FakeModels.Contacts(1).Single();

            await ContactRepository.AddContactAsync(contact);

            // When
            await ContactRepository.DeleteContactByIdAsync(contact.Id);

            // Then
            var contacts = await ContactRepository.FindAllContactsAsync();

            contacts.Any(c => c.Id == contact.Id).Should().BeFalse();
        }

        [Fact]
        public async Task FindAllContactsAsync_ShouldReturnAllContacts()
        {
            // Given
            var expectedContacts = FakeModels.Contacts(5);

            ContactRepository.DeleteAllContacts();
            ContactRepository.AddContacts(expectedContacts);

            // When
            var actualContacts = await ContactRepository.FindAllContactsAsync();

            // Then
            actualContacts.ShouldBeEquivalentTo(actualContacts);
        }
    }
}
