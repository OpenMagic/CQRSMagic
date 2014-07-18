using System;
using CQRSMagic.IoC;
using Ninject;

namespace CQRSMagic.Specifications.Support.IoC
{
    public class DependencyContainer : IDependencyContainer
    {
        private readonly IKernel Kernel;

        public DependencyContainer(IKernel kernel)
        {
            Kernel = kernel;
        }

        public object Get(Type type)
        {
            return Kernel.Get(type);
        }

        public void Bind(Type type, Func<IDependencyContainer, object> toValueFactory)
        {
            Kernel.Bind(type).ToMethod(c => toValueFactory(this));
        }
    }
}
