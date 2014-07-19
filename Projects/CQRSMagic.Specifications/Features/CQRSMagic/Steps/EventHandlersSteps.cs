using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CQRSMagic.Event;
using ExampleDomain.Contacts.Commands;
using ExampleDomain.Contacts.Events;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Practices.ServiceLocation;
using TechTalk.SpecFlow;

namespace CQRSMagic.Specifications.Features.CQRSMagic.Steps
{
    [Binding]
    public class EventHandlersSteps
    {
        private EventHandlers EventHandlers;
        private KeyValuePair<Type, IEnumerable<Func<IEvent, Task>>>[] Handlers;
        private Assembly SearchAssembly;
        private IServiceLocator ServiceLocator;

        [Given(@"ServiceLocator")]
        public void GivenServiceLocator()
        {
            ServiceLocator = A.Fake<IServiceLocator>();
        }

        [Given(@"EventHandlers")]
        public void GivenEventHandlers()
        {
            EventHandlers = new EventHandlers(ServiceLocator);
        }

        [Given(@"an assembly with ISubscribeTo handlers")]
        public void GivenAnAssemblyWithISubscribeToHandlers()
        {
            SearchAssembly = typeof(CreateContact).Assembly;
        }

        [When(@"RegisterEventHandlers is called")]
        public void WhenRegisterEventHandlersIsCalled()
        {
            Handlers = EventHandlers.RegisterHandlers(SearchAssembly).ToArray();
        }

        [Then(@"one handler is created for each ISubscribeTo handler")]
        public void ThenOneHandlerIsCreatedForEachISubscribeToHandler()
        {
            Handlers.Select(x => x.Key).ShouldBeEquivalentTo(new[] {typeof(CreatedContact)});

            var createdContactHandlers = Handlers.Single(x => x.Key == typeof(CreatedContact)).Value.ToArray();

            createdContactHandlers.Length.Should().Be(1);
        }
    }
}