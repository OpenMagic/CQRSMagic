using System;
using CQRSMagic;
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

        public object GetService(Type serviceType)
        {
            return Kernel.GetService(serviceType);
        }
    }
}