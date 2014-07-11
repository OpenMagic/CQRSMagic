using CommonServiceLocator.NinjectAdapter.Unofficial;
using CQRSMagic;
using CQRSMagic.Commands;
using CQRSMagic.Events.Messaging;
using CQRSMagic.Events.Publishing;
using CQRSMagic.Events.Sourcing;
using CQRSMagic.Events.Sourcing.Repositories;
using ExampleDomain.Configuration.Commands;
using Microsoft.Practices.ServiceLocation;
using Ninject;

namespace ExampleMVCApplication
{
    internal static class NinjectConfig
    {
        /// <summary>
        /// Registers services for this web application.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        internal static void RegisterServices(IKernel kernel)
        {
            ServiceLocator.SetLocatorProvider(() => new NinjectServiceLocator(kernel));

            var domainAssemblies = new[] {typeof(ClearAll).Assembly};

            kernel.Bind<ICommandBus>().To<CommandBus>();
            kernel.Bind<ICommandHandlers>().ToConstant(new CommandHandlers(domainAssemblies));
            kernel.Bind<IMessageBus>().To<MessageBus>();
            kernel.Bind<IEventBus>().To<EventBus>();
            kernel.Bind<IEventPublisher>().To<EventPublisher>();
            kernel.Bind<IEventStore>().To<EventStore>();
            kernel.Bind<IEventStoreRepository>().ToConstant(new InMemoryEventStoreRepository()); // Constant is used because is a memory only database.
            kernel.Bind<ISubscriptionHandlers>().ToConstant(new SubscriptionHandlers(domainAssemblies));
        }
    }
}