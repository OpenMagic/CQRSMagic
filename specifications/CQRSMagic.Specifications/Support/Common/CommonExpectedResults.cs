using System.Linq;
using System.Reflection;
using CQRSMagic.Specifications.Support.Dummies;
using FakeItEasy;

namespace CQRSMagic.Specifications.Support.Common
{
    internal static class CommonExpectedResults
    {
        internal static ISubscriberInfo[] Subscribers =
        {
            new SubscriberInfo(
                typeof (DummySubscription1).GetTypeInfo(),
                new[]
                {
                    typeof (ISubscribeTo<DummyEvent1>),
                    typeof (ISubscribeTo<DummyEvent2>),
                    typeof (ISubscribeTo<DummyEvent3>)
                }),
            new SubscriberInfo(
                typeof (DummySubscription2).GetTypeInfo(),
                new[]
                {
                    typeof (ISubscribeTo<DummyEvent1>),
                    typeof (ISubscribeTo<DummyEvent2>),
                    typeof (ISubscribeTo<DummyEvent3>)
                })
        };

        internal static int SubscriberEventCount
        {
            get
            {
                return Subscribers.Sum(subscriberInfo => subscriberInfo.ImplementedInterfaces.Length);
            }
        }
    }
}
