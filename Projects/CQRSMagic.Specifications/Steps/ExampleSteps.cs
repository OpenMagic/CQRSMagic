using System.Linq;
using ExampleDomain.Contacts;
using ExampleDomain.Contacts.Commands;
using ExampleDomain.Contacts.Events;
using ExampleDomain.Contacts.Queries;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace CQRSMagic.Specifications.Steps
{
    [Binding]
    public class ExampleSteps
    {
        private AddContact AddContactCommand;
        private string ContactEmailAddress;
        private string ContactName;
        private IMessageBus MessageBus;
        private IEventStore EventStore;
        private ContactQuery ContactQuery;
        private IEvent[] Events;

        public ExampleSteps()
        {
            EventStore = new EventStore();
            MessageBus = new MessageBus(EventStore);
            ContactQuery = new ContactQuery();
        }

        [Given(@"contact\'s name is (.*)")]
        public void GivenContactsNameIs(string contactName)
        {
            ContactName = contactName;
        }

        [Given(@"their email address is (.*)")]
        public void GivenTheirEmailAddressIs(string contactEmailAddress)
        {
            ContactEmailAddress = contactEmailAddress;
        }

        [When(@"I send AddContact command")]
        public void WhenISendAddContactCommand()
        {
            AddContactCommand = new AddContact
            {
                Name = ContactName,
                EmailAddress = ContactEmailAddress
            };

            Events = MessageBus.Send(AddContactCommand).ToArray();
        }

        [Then(@"ContactAdded event is added to event store")]
        public void ThenContactAddedEventIsAddedToEventStore()
        {
            Events.Length.Should().Be(1);
            Events[0].GetType().Should().Be(typeof(ContactAdded));
        }

        [Then(@"ContactAggregate can be retrieved from event store")]
        public void ThenContactAggregateCanBeRetrievedFromEventStore()
        {
            var contactAggregate = EventStore.GetAggregate<ContactAggregate>(AddContactCommand.AggregateId);

            contactAggregate.Id.Should().Be(AddContactCommand.AggregateId);
            contactAggregate.Name.Should().Be(ContactName);
            contactAggregate.EmailAddress.Should().Be(ContactEmailAddress);
        }

        [Then(@"ContactQueryModel is added to Contacts table")]
        public void ThenContactQueryModelIsAddedToContactsTable()
        {
            var contactViewModel = ContactQuery.GetByEmailAddress(ContactEmailAddress);

            contactViewModel.Id.Should().Be(AddContactCommand.AggregateId);
            contactViewModel.Name.Should().Be(ContactName);
            contactViewModel.EmailAddress.Should().Be(ContactEmailAddress);
        }
    }
}