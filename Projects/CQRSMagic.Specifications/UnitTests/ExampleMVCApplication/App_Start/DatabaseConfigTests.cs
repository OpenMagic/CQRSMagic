using System.Linq;
using CQRSMagic.Specifications.Support;
using ExampleMVCApplication;
using FluentAssertions;
using Xunit;

namespace CQRSMagic.Specifications.UnitTests.ExampleMVCApplication.App_Start
{
    public class DatabaseConfigTests : ExampleMvcApplicationTestBase
    {
        public class InitializeDatabase : DatabaseConfigTests
        {
            [Fact]
            public void ShouldInitializeDatabaseWithTwoContacts()
            {
                // Given
                MessageBus.SendCommands(FakeCommands.AddContactCommands(4));

                // When
                DatabaseConfig.InitializeDatabase(MessageBus);

                // Then
                var contacts = ContactRepository.FindAllContactsAsync().Result.ToArray();

                contacts.Select(c => c.EmailAddress).ShouldAllBeEquivalentTo(new[] { "tim@example.com", "nicole@example.com" });
                contacts.Select(c => c.Name).ShouldAllBeEquivalentTo(new[] { "Tim", "Nicole" });
            }
        }
    }
}
