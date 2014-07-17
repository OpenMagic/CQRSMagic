using System;
using System.Linq;
using System.Threading.Tasks;
using AzureMagic;
using CQRSMagic.Command;
using CQRSMagic.Event;
using CQRSMagic.EventStorage;
using ExampleDomain.Contacts;
using ExampleDomain.Contacts.Commands;
using ExampleDomain.Contacts.Events;
using ExampleDomain.Support;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Ninject;
using TechTalk.SpecFlow;

namespace CQRSMagic.Specifications.Features.ExampleDomain.Steps
{
    [Binding]
    public class ContactsSteps : IDisposable
    {
        private readonly ICommandBus CommandBus;
        private readonly Guid ContactId;
        private readonly IContactRepository ContactRepository;
        private readonly IEventStore EventStore;
        private readonly TableNameFormatter TableNameFormatter;

        private string EmailAddress;
        private string Name;
        private bool IsDisposed;

        public ContactsSteps()
        {
            var tableClient = AzureStorage.GetTableClient(AzureStorage.DevelopmentConnectionString);
            TableNameFormatter = new TableNameFormatter(tableClient, GetType().Name);
            var kernel = IoC.RegisterServices(new StandardKernel(), tableClient, TableNameFormatter);
            var dependencyResolver = kernel.Get<IDependencyResolver>();

            IEventBus eventBus = new EventBus(dependencyResolver);

            ContactId = Guid.NewGuid();
            EventStore = new EventStore(kernel.Get<IEventStoreRepository>(), dependencyResolver);
            CommandBus = new CommandBus(EventStore, eventBus, dependencyResolver);

            kernel.Bind<IEventStore>().ToConstant(EventStore);

            ContactRepository = kernel.Get<IContactRepository>();

            CommandBus.RegisterHandlers(typeof(CreateContact).Assembly);
            eventBus.RegisterHandlers(typeof(CreatedContact).Assembly);
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and optionally managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    TableNameFormatter.Dispose();
                }
            }

            IsDisposed = true;
        }

        private EquivalencyAssertionOptions<IEvent> EventEquivalencyOptions(EquivalencyAssertionOptions<IEvent> options)
        {
            var excludedEntityProperties = new[] { "EventCreated"  };

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
            var addContact = new CreateContact {Id = ContactId, Name = Name, EmailAddress = EmailAddress};
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