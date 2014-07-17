using System;
using CQRSMagic.Command;
using CQRSMagic.Event;
using CQRSMagic.EventStorage;
using ExampleDomain.Contacts;
using ExampleDomain.Contacts.Commands;
using ExampleDomain.Contacts.Events;
using ExampleDomain.Support;
using FluentAssertions;
using Ninject;
using TechTalk.SpecFlow;

namespace CQRSMagic.Specifications.Features.ExampleDomain.Steps
{
    [Binding]
    public class ContactsSteps
    {
        private readonly ICommandBus CommandBus;
        private readonly Guid ContactId;
        private readonly IContactRepository ContactRepository;
        private readonly IEventStore EventStore;

        private string EmailAddress;
        private string Name;

        public ContactsSteps()
        {
            var kernel = IoC.RegisterServices(new StandardKernel());
            var dependencyResolver = kernel.Get<IDependencyResolver>();

            IEventBus eventBus = new EventBus(dependencyResolver);

            ContactId = Guid.NewGuid();
            EventStore = new EventStore(kernel.Get<IEventStoreRepository>(), dependencyResolver);
            CommandBus = new CommandBus(EventStore, eventBus, dependencyResolver);

            kernel.Bind<IEventStore>().ToConstant(EventStore);

            ContactRepository = kernel.Get<IContactRepository>();

            CommandBus.RegisterHandlers(typeof(AddContact).Assembly);
            eventBus.RegisterHandlers(typeof(ContactAdded).Assembly);
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
            var addContact = new AddContact {Id = ContactId, Name = Name, EmailAddress = EmailAddress};
            CommandBus.SendCommandAsync(addContact).Wait();
        }

        [Then(@"ContactAdded event is added to the event store")]
        public void ThenContactAddedEventIsAddedToTheEventStore()
        {
            var actualEvents = EventStore.FindEventsAsync(ContactId).Result;
            var expectedEvents = new IEvent[] {new ContactAdded(ContactId, Name, EmailAddress)};

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