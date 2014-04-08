using CQRS.Example.Support;
using Microsoft.Practices.ServiceLocation;
using TinyIoC;

namespace CQRS.Example.Configure
{
    internal static class ContainerConfiguration
    {
        internal static IServiceLocator Configure()
        {
            var container = TinyIoCContainer.Current;

            container.AutoRegister();
            container.Register((c, p) => ServiceLocator.Current);

            ServiceLocator.SetLocatorProvider(() => new TinyIoCServiceLocator(container));

            return ServiceLocator.Current;
        }
    }
}