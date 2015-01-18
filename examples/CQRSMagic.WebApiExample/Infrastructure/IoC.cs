using CQRSMagic.WebApiExample.Products;
using Ninject;

namespace CQRSMagic.WebApiExample.Infrastructure
{
    // NOTE: This pure infrastructure code. No need to read it to understand CQRSMagic.
    internal static class IoC
    {
        internal static readonly IKernel Kernel = CreateKernel();

        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();

            kernel.Bind<IEventPublisher>().To<EventPublisher>().InSingletonScope();
            kernel.Bind<IEventStore>().To<InMemoryEventStore>();
            kernel.Bind<IProductReadModelRepository>().To<ProductReadModelRepository>();

            return kernel;
        }
    }
}
