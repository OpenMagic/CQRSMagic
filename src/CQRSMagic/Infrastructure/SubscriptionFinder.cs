using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Common.Logging;
using CQRSMagic.Logging;
using CQRSMagic.Support;

namespace CQRSMagic.Infrastructure
{
    public class SubscriptionFinder : ISubscriptionFinder
    {
        private static readonly ILog Log = LogManager.GetLogger<SubscriptionFinder>();

        public ISubscriberInfo[] FindSubscribers(Assembly assembly)
        {
            var sw = Stopwatch.StartNew();

            var subscribers = (
                from exportedType in assembly.ExportedTypes
                let typeInfo = exportedType.GetTypeInfo()
                where typeInfo.IsConcreteType()
                let implementedInterfaces = (
                    from implementedInterface in typeInfo.ImplementedInterfaces
                    where implementedInterface.Implements(typeof (ISubscribeTo<>))
                    select implementedInterface).ToArray()
                where implementedInterfaces.Length > 0
                select (ISubscriberInfo) new SubscriberInfo(typeInfo, implementedInterfaces)).ToArray();

            Log.ElapsedTime("FindSubscribers(Assembly assembly)", sw.Elapsed, 20);

            return subscribers;
        }
    }
}