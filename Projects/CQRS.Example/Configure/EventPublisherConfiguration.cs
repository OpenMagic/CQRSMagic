using System.Threading.Tasks;
using CQRS.Example.CQRS;
using CQRS.Example.CQRS.Events;
using CQRS.Example.Customers.Commands;
using CQRS.Example.Customers.Domain;
using CQRS.Example.Customers.Events;

namespace CQRS.Example.Configure
{
    internal class EventPublisherConfiguration
    {
        internal static Task RegisterEventHandlers(IEventPublisher eventPublisher)
        {
            return Task.Factory.StartNew(() => RegisterEventHandlersSync(eventPublisher));
        }

        private static void RegisterEventHandlersSync(IEventPublisher eventPublisher)
        {
            eventPublisher.RegisterHandler<CustomerEventHandler, CreatedCustomer>();
            eventPublisher.RegisterHandler<CustomerEventHandler, RenamedCustomer>();
        }
    }
}