#define AzureEventStoreRepository

using System;
using System.Linq;
using CommonServiceLocator.NinjectAdapter.Unofficial;
using CQRSMagic.Azure;
using CQRSMagic.Azure.Support;
using CQRSMagic.Commands;
using CQRSMagic.Events.Messaging;
using CQRSMagic.Events.Publishing;
using CQRSMagic.Events.Sourcing;
using CQRSMagic.Events.Sourcing.Repositories;
using CQRSMagic.Specifications.Steps.Support;
using ExampleDomain.Contacts;
using ExampleDomain.Contacts.Commands;
using ExampleDomain.Contacts.Events;
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
        private static readonly string AzureContactsTableName = string.Format("TestContacts{0}", Guid.NewGuid().ToString().Replace("-", ""));
        private static readonly string AzureEventsTableName = string.Format("TestEvents{0}", Guid.NewGuid().ToString().Replace("-", ""));
        private const string AzureEventStoreConnectionString = AzureStorage.DevelopmentConnectionString;

        private readonly IContactRepository ContactRepository;
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

            ServiceLocator.SetLocatorProvider(() => new NinjectServiceLocator(kernel));

#if AzureEventStoreRepository

            kernel.Bind<IContactRepository>().ToConstructor(cas => new AzureContactRepository(AzureEventStoreConnectionString, AzureContactsTableName));
            kernel.Bind<IEventStoreRepository>().ToConstructor(cas => new AzureEventStoreRepository(AzureEventStoreConnectionString, AzureEventsTableName));
            kernel.Bind<IAzureEventSerializer>().To<AzureEventSerializer>();

#else

            kernel.Bind<IContactRepository>().ToConstant(new InMemoryContactRepository());
            kernel.Bind<IEventStoreRepository>().ToConstant(new InMemoryEventStoreRepository());

#endif

            EventStoreRepository = kernel.Get<IEventStoreRepository>();
            EventStore = new EventStore(EventStoreRepository);

            var domainAssemblies = new[] {typeof(AddContact).Assembly};
            var contactRepository = kernel.Get<IContactRepository>();

            var commandHandlers = new CommandHandlers(domainAssemblies);
            var commandBus = new CommandBus(commandHandlers);

            var eventBus = new EventBus(EventStore);

            var subscriptionHandlers = new SubscriptionHandlers(domainAssemblies);
            var eventPublisher = new EventPublisher(subscriptionHandlers);

            MessageBus = new MessageBus(commandBus, eventBus, eventPublisher);
            ContactRepository = ServiceLocator.Current.GetInstance<IContactRepository>();
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

            Events = MessageBus.SendCommandAsync(AddContactCommand).Result.ToArray();
        }

        [Then(@"ContactAdded event is added to event store")]
        public void ThenContactAddedEventIsAddedToEventStore()
        {
            Events.Length.Should().Be(1);
            Events[0].GetType().Should().Be(typeof(AddedContact));

            var storedEvents = EventStoreRepository.GetEventsAsync<ContactAggregate>(AddContactCommand.AggregateId).Result;

            storedEvents.ShouldBeEquivalentTo(Events);
        }

        [Then(@"ContactAggregate can be retrieved from event store")]
        public void ThenContactAggregateCanBeRetrievedFromEventStore()
        {
            var contactAggregate = EventStore.GetAggregateAsync<ContactAggregate>(AddContactCommand.AggregateId).Result;

            contactAggregate.Id.Should().Be(AddContactCommand.AggregateId);
            contactAggregate.Name.Should().Be(ContactName);
            contactAggregate.EmailAddress.Should().Be(ContactEmailAddress);
        }

        [Then(@"ContactQueryModel is added to Contacts table")]
        public void ThenContactQueryModelIsAddedToContactsTable()
        {
            var contactViewModel = ContactRepository.GetByEmailAddressAsync(ContactEmailAddress).Result;

            contactViewModel.Id.Should().Be(AddContactCommand.AggregateId);
            contactViewModel.Name.Should().Be(ContactName);
            contactViewModel.EmailAddress.Should().Be(ContactEmailAddress);
        }

        [AfterScenario]
        public static void AfterScenario()
        {
            AzureStorage.DeleteTableIfExists(AzureEventStoreConnectionString, AzureContactsTableName);
            AzureStorage.DeleteTableIfExists(AzureEventStoreConnectionString, AzureEventsTableName);
        }
    }
}