using System.Reflection;

namespace CQRSMagic
{
    public interface ISubscriptionFinder
    {
        ISubscriberInfo[] FindSubscribers(Assembly assembly);
    }
}
