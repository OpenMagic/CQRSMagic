using System.Reflection;

namespace CQRSMagic.Infrastructure
{
    public interface IAssemblyEventSubscriber
    {
        void SubscribeEventHandlers(Assembly inAssembly, IEventPublisher toEventPublisher);
    }
}