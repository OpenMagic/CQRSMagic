using CQRSMagic;
using CQRSMagic.EventStorage;
using ExampleDomain.Contacts;
using ExampleDomain.Repositories.InMemory;
using Ninject;

namespace ExampleDomain.Support
{
    public class IoC
    {
        public static IKernel RegisterServices(IKernel kernel)
        {
            kernel.Bind<IDependencyResolver>().ToConstant(new DependencyResolver(kernel));

            RegisterInMemoryRepositories(kernel);

            return kernel;
        }

        private static void RegisterInMemoryRepositories(IKernel kernel)
        {
            kernel.Bind<IContactRepository>().To<InMemoryContactRepository>().InSingletonScope();
            kernel.Bind<IEventStoreRepository>().To<InMemoryEventStoreRepository>().InSingletonScope();
        }
    }
}