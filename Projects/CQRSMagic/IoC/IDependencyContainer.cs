using System;

namespace CQRSMagic.IoC
{
    public interface IDependencyContainer : IDependencyResolver
    {
        void Bind(Type type, Func<IDependencyContainer, object> toValueFactory);
    }
}
