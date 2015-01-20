using Common.Logging;
using CQRSMagic.Logging;

namespace CQRSMagic.Specifications.Support.Dummies
{
    // A collection of methods that subscribe to events.
    public class Subscriptions :
        ISubscribeTo<Event1>,
        ISubscribeTo<Event2>,
        ISubscribeTo<Event3>
    {
        private static readonly ILog Log = LogManager.GetLogger<Subscriptions>();

        public void SubscriptionHandler(Event1 e)
        {
            LogSubscriptionHandler(e);
        }

        public void SubscriptionHandler(Event2 e)
        {
            LogSubscriptionHandler(e);
        }

        public void SubscriptionHandler(Event3 e)
        {
            LogSubscriptionHandler(e);
        }

        private static void LogSubscriptionHandler(IEvent e)
        {
            Log.Trace("SubscriptionHandler({0} e)", e.GetType().Name);
        }
    }
}
