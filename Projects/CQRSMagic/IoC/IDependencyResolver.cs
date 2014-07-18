using System;

namespace CQRSMagic.IoC
{
    public interface IDependencyResolver
    {
        object Get(Type type);
    }
}