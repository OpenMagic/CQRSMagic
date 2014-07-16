using CQRSMagic.Domain;
using CQRSMagic.EventStorage;
using ExampleDomain.Contacts;
using ExampleDomain.Repositories.InMemory;
using Ninject;

namespace ExampleDomain
{
    public class IoC
    {
        public static IKernel RegisterServices(IKernel kernel)
        {
            kernel.Bind<IAggregateFactory>().To<AggregateFactory>();

            RegisterInMemoryRepositories(kernel);

            return kernel;
        }

        private static void RegisterInMemoryRepositories(IKernel kernel)
        {
            kernel.Bind<IContactRepository>().To<InMemoryContactRepository>();
            kernel.Bind<IEventStoreRepository>().To<InMemoryEventStoreRepository>();
        }
    }
}