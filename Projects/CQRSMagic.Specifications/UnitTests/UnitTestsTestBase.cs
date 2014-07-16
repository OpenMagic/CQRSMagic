using System;
using CQRSMagic.Specifications.Support;
using ExampleDomain.Contacts.Queries.Repositories;
using ExampleDomain.Repositories;
using ExampleDomain.Repositories.InMemory;
using ExampleMVCApplication;
using Microsoft.Practices.ServiceLocation;
using Ninject;

namespace CQRSMagic.Specifications.UnitTests
{
    public abstract class UnitTestsTestBase : TestBase
    {
        private readonly Func<IKernel, IRepositoryFactory> CreateRepositoryFactory;
        private bool RegisteredServices;

        protected UnitTestsTestBase()
            : this(kernel => new InMemoryRepositories())
        {
        }

        protected UnitTestsTestBase(Func<IKernel, IRepositoryFactory> createRepositoryFactory)
        {
            CreateRepositoryFactory = createRepositoryFactory;
        }

        protected IContactRepository ContactRepository
        {
            get { return GetService<IContactRepository>(); }
        }

        protected IMessageBus MessageBus
        {
            get { return GetService<IMessageBus>(); }
        }

        private TService GetService<TService>()
        {
            if (!RegisteredServices)
            {
                NinjectConfig.RegisterServices(new StandardKernel(), CreateRepositoryFactory);
                RegisteredServices = true;
            }

            return ServiceLocator.Current.GetInstance<TService>();
        }
    }
}