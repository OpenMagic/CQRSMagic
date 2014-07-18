using System;
using CQRSMagic;
using CQRSMagic.IoC;
using Ninject;

namespace ExampleDomain.Support
{
    public class DependencyResolver : IDependencyResolver
    {
        private readonly IKernel Kernel;

        public DependencyResolver(IKernel kernel)
        {
            Kernel = kernel;
        }

        public object Get(Type type)
        {
            return Kernel.GetService(type);
        }
    }
}