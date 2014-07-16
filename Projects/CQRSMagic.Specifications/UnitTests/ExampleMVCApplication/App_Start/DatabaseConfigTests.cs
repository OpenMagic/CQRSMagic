using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Anotar.CommonLogging;
using CQRSMagic.Specifications.Support;
using ExampleDomain.Contacts.Queries.Models;
using ExampleDomain.Repositories;
using ExampleMVCApplication;
using FluentAssertions;
using Ninject;
using Xunit;

namespace CQRSMagic.Specifications.UnitTests.ExampleMVCApplication.App_Start
{
    public class DatabaseConfigTests : ExampleMvcApplicationTestBase
    {
        public DatabaseConfigTests()
        {
        }

        protected DatabaseConfigTests(Func<IKernel, IRepositoryFactory> createRepositoryFactory)
            : base(createRepositoryFactory)
        {
        }

        public class InitializeDatabase : DatabaseConfigTests
        {
            public InitializeDatabase()
                : base(RepositoryFactories.AzureRepositories)
            {
            }

            [Fact]
            public void ShouldInitializeDatabaseWithTwoContacts()
            {
                // Given
                MessageBus.SendCommands(FakeCommands.AddContactCommands(4));

                // When
                DatabaseConfig.InitializeDatabase(MessageBus);

                // Then
                var contacts = GetContacts(2);

                contacts.Select(c => c.EmailAddress).ShouldAllBeEquivalentTo(new[] { "tim@example.com", "nicole@example.com" });
                contacts.Select(c => c.Name).ShouldAllBeEquivalentTo(new[] { "Tim", "Nicole" });
            }

            private ContactReadModel[] GetContacts(int expectedCount)
            {
                var sw = Stopwatch.StartNew();
                var contacts = new ContactReadModel[] {};

                while (sw.ElapsedMilliseconds < 5000 && contacts.Length != expectedCount)
                {
                    Thread.Sleep(0);
                    contacts = ContactRepository.FindAllContactsAsync().Result.ToArray();
                }

                return contacts;
            }
        }
    }
}