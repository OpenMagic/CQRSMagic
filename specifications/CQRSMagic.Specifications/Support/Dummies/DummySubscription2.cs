using Common.Logging;
using CQRSMagic.Logging;

namespace CQRSMagic.Specifications.Support.Dummies
{
    public class DummySubscription2 :
        ISubscribeTo<DummyEvent1>,
        ISubscribeTo<DummyEvent2>,
        ISubscribeTo<DummyEvent3>
    {
        private static readonly ILog Log = LogManager.GetLogger<DummySubscription2>();

        public void SubscriptionHandler(DummyEvent1 e)
        {
            LogSubscriptionHandler(e);
        }

        public void SubscriptionHandler(DummyEvent2 e)
        {
            LogSubscriptionHandler(e);
        }

        public void SubscriptionHandler(DummyEvent3 e)
        {
            LogSubscriptionHandler(e);
        }

        private static void LogSubscriptionHandler(IEvent e)
        {
            Log.Trace("SubscriptionHandler({0} e)", e.GetType().Name);
        }
    }
}
