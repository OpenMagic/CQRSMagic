using System;

namespace CQRSMagic.IoC
{
    public static class DependencyContainerExtensions
    {
        public static void Bind<T>(this IDependencyContainer container, Func<IDependencyContainer, object> toValueFactory)
        {
            container.Bind(typeof(T), toValueFactory);
        }
    }
}