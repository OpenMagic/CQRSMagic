using System;
using CQRSMagic.Command;
using CQRSMagic.Event;
using CQRSMagic.EventStorage;
using ExampleDomain.Contacts;
using ExampleDomain.Contacts.Commands;
using ExampleDomain.Contacts.Events;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace CQRSMagic.Specifications.Features.Example.Steps
{
    [Binding]
    public class ContactsSteps
    {
        private readonly Guid ContactId;
        private readonly ICommandBus CommandBus;
        private readonly IEventStore EventStore;
        private readonly IContactRepository ContactRepository;

        private string EmailAddress;
        private string Name;

        public ContactsSteps()
        {
            ContactId = Guid.NewGuid();
            CommandBus = new CommandBus();
            EventStore = new EventStore();
            ContactRepository = new ContactRepository();
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
            var addContact = new AddContact { Id = Guid.NewGuid(), Name = Name, EmailAddress = EmailAddress };
            CommandBus.SendCommandAsync(addContact).Wait();
        }

        [Then(@"ContactAdded event is added to the event store")]
        public void ThenContactAddedEventIsAddedToTheEventStore()
        {
            var actualEvents = EventStore.FindEventsAsync(ContactId).Result;
            var expectedEvents = new IEvent[] { new ContactAdded { Id = ContactId, Name = Name, EmailAddress = EmailAddress } };

            expectedEvents.ShouldAllBeEquivalentTo(actualEvents);
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