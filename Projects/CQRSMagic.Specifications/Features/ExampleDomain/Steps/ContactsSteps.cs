using System;
using System.Linq;
using System.Threading.Tasks;
using CQRSMagic.Command;
using CQRSMagic.Event;
using CQRSMagic.EventStorage;
using CQRSMagic.Specifications.Support.Configurations;
using ExampleDomain.Contacts;
using ExampleDomain.Contacts.Commands;
using ExampleDomain.Contacts.Events;
using FluentAssertions;
using FluentAssertions.Equivalency;
using TechTalk.SpecFlow;

namespace CQRSMagic.Specifications.Features.ExampleDomain.Steps
{
    [Binding]
    public class ContactsSteps : IDisposable
    {
        private readonly ICommandBus CommandBus;
        private readonly AzureConfiguration Configuration;
        private readonly Guid ContactId;
        private readonly IContactRepository ContactRepository;
        private readonly IEventStore EventStore;

        private string EmailAddress;
        private bool IsDisposed;
        private string Name;

        public ContactsSteps()
        {
            Configuration = new AzureConfiguration(GetType().Name);

            ContactId = Guid.NewGuid();
            EventStore = Configuration.Get<IEventStore>();
            CommandBus = Configuration.Get<ICommandBus>();
            ContactRepository = Configuration.Get<IContactRepository>();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Releases unmanaged and optionally managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
        ///     unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    Configuration.CleanUp();
                }
            }
            IsDisposed = true;
        }

        private EquivalencyAssertionOptions<IEvent> EventEquivalencyOptions(EquivalencyAssertionOptions<IEvent> options)
        {
            var excludedEntityProperties = new[] {"EventCreated"};

            return options.Excluding(subjectInfo => excludedEntityProperties.Contains(subjectInfo.PropertyInfo.Name));
        }

        [Given(@"name is (.*)")]
        public void GivenNameIs(string name)
        {
            Name = name;
        }

        [Given(@"emailAddress is (.*)")]
        public void GivenEmailAddressIs(string emailAddress)
        {
            EmailAddress = emailAddress;
        }

        [When(@"AddContact command is sent")]
        public void WhenAddContactCommandIsSent()
        {
            var addContact = new CreateContact {AggregateId = ContactId, Name = Name, EmailAddress = EmailAddress};
            var tasks = CommandBus.SendCommandAsync(addContact).Result;

            Task.WaitAll(tasks.ToArray());
        }

        [Then(@"ContactAdded event is added to the event store")]
        public void ThenContactAddedEventIsAddedToTheEventStore()
        {
            var actualEvents = EventStore.FindEventsAsync(ContactId).Result;
            var expectedEvents = new IEvent[] {new CreatedContact(ContactId, Name, EmailAddress)};

            expectedEvents.ShouldAllBeEquivalentTo(actualEvents, EventEquivalencyOptions);
        }

        [Then(@"ContactAggregate can be read from the event store")]
        public void ThenContactAggregateCanBeReadFromTheEventStore()
        {
            var contact = EventStore.GetAggregateAsync<ContactAggregate>(ContactId).Result;

            contact.Id.Should().Be(ContactId);
            contact.Name.Should().Be(Name);
            contact.EmailAddress.Should().Be(EmailAddress);
        }

        [Then(@"Contact is added to ContactRepository")]
        public void ThenContactIsAddedToContactRepository()
        {
            var contact = ContactRepository.GetContactAsync(ContactId).Result;

            contact.Id.Should().Be(ContactId);
            contact.Name.Should().Be(Name);
            contact.EmailAddress.Should().Be(EmailAddress);
        }
    }
}