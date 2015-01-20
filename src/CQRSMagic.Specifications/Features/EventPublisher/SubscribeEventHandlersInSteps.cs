using System.Reflection;
using CQRSMagic.Infrastructure;
using FakeItEasy;
using TechTalk.SpecFlow;

namespace CQRSMagic.Specifications.Features.EventPublisher
{
    [Binding]
    public class SubscribeEventHandlersInSteps
    {
        private CQRSMagic.EventPublisher _eventPublisher;
        private Assembly _assembly;
        private IAssemblyEventSubscriber _assemblyEventSubscriber;

        [Given(@"I have a new EventPublisher")]
        public void GivenIHaveANewEventPublisher()
        {
            _assemblyEventSubscriber = A.Fake<IAssemblyEventSubscriber>();
            _eventPublisher = new CQRSMagic.EventPublisher(_assemblyEventSubscriber);
        }

        [Given(@"I have an application with event handlers")]
        public void GivenIHaveAnApplicationWithEventHandlers()
        {
            _assembly = Assembly.GetExecutingAssembly();
        }

        [When(@"I call EventPublisher\.SubscribeEventHandlersIn")]
        public void WhenICallEventPublisher_SubscribeEventHandlersIn()
        {
            _eventPublisher.SubscribeEventHandlersIn(_assembly);
        }

        [Then(@"all event handlers are subscribed")]
        public void ThenAllEventHandlersAreSubscribed()
        {
            A.CallTo(() => _assemblyEventSubscriber.SubscribeEventHandlers(_assembly, _eventPublisher)).MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}
