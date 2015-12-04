using System;
using CQRSMagic.Specifications.Support;
using CQRSMagic.Specifications.Support.Common;
using CQRSMagic.Specifications.Support.Dummies;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using TechTalk.SpecFlow;

namespace CQRSMagic.Specifications.Features.Infrastructure.AssemblyEventSubscriber
{
    [Binding]
    public class SubscribeEventHandlersSteps
    {
        private readonly CommonData _commonData;
        private IEventPublisher _eventPublisher;
        private ISubscriptionFinder _subscriptionFinder;
        private CQRSMagic.Infrastructure.AssemblyEventSubscriber _assemblyEventSubsciber;

        public SubscribeEventHandlersSteps(CommonData commonData)
        {
            _commonData = commonData;
        }

        [Given(@"I have a new AssemblyEventSubsciber")]
        public void GivenIHaveANewAssemblyEventSubsciber()
        {
            _subscriptionFinder = A.Fake<ISubscriptionFinder>();

            A.CallTo(() => _subscriptionFinder.FindSubscribers(_commonData.Assembly)).Returns(CommonExpectedResults.Subscribers);

            _assemblyEventSubsciber = new CQRSMagic.Infrastructure.AssemblyEventSubscriber(_subscriptionFinder);
        }

        [Given(@"I have an EventPublisher")]
        public void GivenIHaveAnEventPublisher()
        {
            _eventPublisher = A.Fake<IEventPublisher>();
        }

        [When(@"I call AssemblyEventSubscriber\.SubscribeEventHandlers")]
        public void WhenICall_AssemblyEventSubscriber_SubscribeEventHandlers()
        {
            _assemblyEventSubsciber.SubscribeEventHandlers(_commonData.Assembly, _eventPublisher);
        }

        [Scope(Feature = "SubscribeEventHandlers")]
        [Then(@"all event handlers are subscribed")]
        public void ThenAllEventHandlersAreSubscribed()
        {
            A.CallTo(() => _eventPublisher.SubscribeTo(A<Action<IEvent>>.Ignored)).MustHaveHappened(Repeated.Exactly.Times(CommonExpectedResults.SubscriberEventCount));
        }
    }
}
