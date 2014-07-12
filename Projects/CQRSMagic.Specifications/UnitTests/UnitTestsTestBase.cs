using CQRSMagic.Specifications.Support;
using ExampleDomain.Contacts.Queries.Repositories;
using ExampleDomain.Repositories.InMemory;
using ExampleMVCApplication;
using Microsoft.Practices.ServiceLocation;
using Ninject;

namespace CQRSMagic.Specifications.UnitTests
{
    public abstract class UnitTestsTestBase : TestBase
    {
        private bool RegisteredServices;

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
                NinjectConfig.RegisterServices(new StandardKernel(), new InMemoryRepositories());
                RegisteredServices = true;
            }

            return ServiceLocator.Current.GetInstance<TService>();
        }
    }
}