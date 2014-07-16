using System;
using CQRSMagic.Azure;
using ExampleDomain.Repositories.Azure;
using ExampleDomain.Repositories.InMemory;
using Ninject;

namespace ExampleMVCApplication
{
    public static class RepositoryFactories
    {
        public static readonly Func<IKernel, AzureRepositories> AzureRepositories = kernel =>
        {
            kernel.Bind<IAzureEventSerializer>().To<AzureEventSerializer>();

            return new AzureRepositories();
        };

        public static readonly Func<IKernel, InMemoryRepositories> InMemoryRepositories = kernel => new InMemoryRepositories();
    }
}