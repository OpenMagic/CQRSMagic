using System;

namespace CQRSMagic
{
    public interface IDependencyResolver
    {
        object GetService(Type serviceType);
    }
}
