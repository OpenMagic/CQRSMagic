using CQRSMagic.Domain;
using CQRSMagic.EventStorage;
using ExampleDomain.Repositories.InMemory;
using Ninject;

namespace ExampleDomain
{
    public class IoC
    {
        public static IKernel RegisterServices(IKernel kernel)
        {
            kernel.Bind<IEventStoreRepository>().To<InMemoryEventStoreRepository>();
            kernel.Bind<IAggregateFactory>().To<AggregateFactory>();

            return kernel;
        }
    }
}