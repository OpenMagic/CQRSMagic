using System.Linq;
using CommonServiceLocator.NinjectAdapter.Unofficial;
using CQRSMagic.Commands;
using CQRSMagic.Events;
using CQRSMagic.Events.Publishing;
using CQRSMagic.Specifications.Steps.Support;
using ExampleDomain.Contacts;
using ExampleDomain.Contacts.Commands;
using ExampleDomain.Contacts.Events;
using ExampleDomain.Contacts.Queries;
using ExampleDomain.Contacts.Queries.Repositories;
using FluentAssertions;
using Microsoft.Practices.ServiceLocation;
using Ninject;
using TechTalk.SpecFlow;

namespace CQRSMagic.Specifications.Steps
{
    [Binding]
    public class ExampleSteps
    {
        private readonly ContactQuery ContactQuery;
        private readonly IEventStore EventStore;
        private readonly IEventStoreRepository EventStoreRepository;
        private readonly IMessageBus MessageBus;
        private AddContact AddContactCommand;
        private string ContactEmailAddress;
        private string ContactName;
        private IEvent[] Events;

        public ExampleSteps()
        {
            var kernel = new StandardKernel();

            kernel.Bind<IContactRepository>().ToConstant(new InMemoryContactRepository());

            ServiceLocator.SetLocatorProvider(() => new NinjectServiceLocator(kernel));

            var domainAssemblies = new[] {typeof(AddContact).Assembly};
            var contactRepository = kernel.Get<IContactRepository>();

            EventStoreRepository = new InMemoryEventStoreRepository();
            EventStore = new EventStore(EventStoreRepository);

            var commandHandlers = new CommandHandlers(domainAssemblies);
            var commandBus = new CommandBus(commandHandlers);

            var eventBus = new EventBus(EventStore);

            var subscriptionHandlers = new SubscriptionHandlers(domainAssemblies);
            var eventPublisher = new EventPublisher(subscriptionHandlers);

            MessageBus = new MessageBus(commandBus, eventBus, eventPublisher);
            ContactQuery = new ContactQuery(contactRepository);
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

            Events = MessageBus.SendCommand(AddContactCommand).ToArray();
        }

        [Then(@"ContactAdded event is added to event store")]
        public void ThenContactAddedEventIsAddedToEventStore()
        {
            Events.Length.Should().Be(1);
            Events[0].GetType().Should().Be(typeof(ContactAdded));

            var storedEvents = EventStoreRepository.GetEvents<ContactAggregate>(AddContactCommand.AggregateId);

            storedEvents.ShouldBeEquivalentTo(Events);
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