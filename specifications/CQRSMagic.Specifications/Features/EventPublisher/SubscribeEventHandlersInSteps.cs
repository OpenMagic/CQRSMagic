using CQRSMagic.Infrastructure;
using CQRSMagic.Specifications.Support.Common;
using FakeItEasy;
using TechTalk.SpecFlow;

namespace CQRSMagic.Specifications.Features.EventPublisher
{
    [Binding]
    public class SubscribeEventHandlersInSteps
    {
        private readonly CommonData _commonData;
        private CQRSMagic.EventPublisher _eventPublisher;
        private IAssemblyEventSubscriber _assemblyEventSubscriber;

        public SubscribeEventHandlersInSteps(CommonData commonData)
        {
            _commonData = commonData;
        }

        [Given(@"I have a new EventPublisher")]
        public void GivenIHaveANewEventPublisher()
        {
            _assemblyEventSubscriber = A.Fake<IAssemblyEventSubscriber>();
            _eventPublisher = new CQRSMagic.EventPublisher(_assemblyEventSubscriber);

        }

        [When(@"I call EventPublisher\.SubscribeEventHandlersIn")]
        public void WhenICallEventPublisher_SubscribeEventHandlersIn()
        {
            _eventPublisher.SubscribeEventHandlersIn(_commonData.Assembly);
        }

        [Scope(Feature = "SubscribeEventHandlersIn")]
        [Then(@"all event handlers are subscribed")]
        public void ThenAllEventHandlersAreSubscribed()
        {
            A.CallTo(() => _assemblyEventSubscriber.SubscribeEventHandlers(_commonData.Assembly, _eventPublisher)).MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}
