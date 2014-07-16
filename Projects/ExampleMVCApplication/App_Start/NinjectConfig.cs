using System;
using CommonServiceLocator.NinjectAdapter.Unofficial;
using CQRSMagic;
using CQRSMagic.Commands;
using CQRSMagic.Events.Messaging;
using CQRSMagic.Events.Publishing;
using CQRSMagic.Events.Sourcing;
using ExampleDomain.Configuration.Commands;
using ExampleDomain.Repositories;
using Microsoft.Practices.ServiceLocation;
using Ninject;

namespace ExampleMVCApplication
{
    public static class NinjectConfig
    {
        /// <summary>
        ///     Registers services for this web application.
        /// </summary>
        public static void RegisterServices(IKernel kernel, Func<IKernel, IRepositoryFactory> createRepositoryFactory)
        {
            ServiceLocator.SetLocatorProvider(() => new NinjectServiceLocator(kernel));

            var domainAssemblies = new[] {typeof(ClearAll).Assembly};

            kernel.Bind<ICommandBus>().To<CommandBus>();
            kernel.Bind<ICommandHandlers>().ToConstant(new CommandHandlers(domainAssemblies));
            kernel.Bind<IMessageBus>().To<MessageBus>();
            kernel.Bind<IEventBus>().To<EventBus>();
            kernel.Bind<IEventPublisher>().To<EventPublisher>();
            kernel.Bind<IEventStore>().To<EventStore>();
            kernel.Bind<ISubscriptionHandlers>().ToConstant(new SubscriptionHandlers(domainAssemblies));

            BindRepositories(kernel, createRepositoryFactory(kernel));
        }

        private static void BindRepositories(IKernel kernel, IRepositoryFactory repositoryFactory)
        {
            foreach (var p in repositoryFactory.GetType().GetProperties())
            {
                var property = p;

                kernel.Bind(property.PropertyType).ToMethod(c => property.GetValue(repositoryFactory, null));
            }
        }
    }
}