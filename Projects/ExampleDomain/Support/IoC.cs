using System;
using CQRSMagic;
using CQRSMagic.Azure;
using CQRSMagic.EventStorage;
using ExampleDomain.Contacts;
using ExampleDomain.Repositories.Azure;
using ExampleDomain.Repositories.InMemory;
using Microsoft.WindowsAzure.Storage.Table;
using Ninject;

namespace ExampleDomain.Support
{
    public class IoC
    {
        public static IKernel RegisterServices(IKernel kernel)
        {
            return RegisterServices(kernel, RegisterInMemoryRepositories);
        }

        public static IKernel RegisterServices(IKernel kernel, CloudTableClient tableClient)
        {
            return RegisterServices(kernel, tableClient, new TableNameFormatter(tableClient, null));
        }

        public static IKernel RegisterServices(IKernel kernel, CloudTableClient tableClient, TableNameFormatter tableNameFormatter)
        {
            return RegisterServices(kernel, k => RegisterAzureTableRepositories(k, tableClient, tableNameFormatter));
        }

        public static IKernel RegisterServices(IKernel kernel, Action<IKernel> registerRepositories)
        {
            kernel.Bind<IDependencyResolver>().ToConstant(new DependencyResolver(kernel));

            registerRepositories(kernel);

            return kernel;
        }

        private static void RegisterInMemoryRepositories(IKernel kernel)
        {
            kernel.Bind<IContactRepository>().To<InMemoryContactRepository>().InSingletonScope();
            kernel.Bind<IEventStoreRepository>().To<InMemoryEventStoreRepository>().InSingletonScope();
        }

        private static void RegisterAzureTableRepositories(IKernel kernel, CloudTableClient tableClient, TableNameFormatter tableNameFormatter)
        {
            kernel.Bind<IAzureEventSerializer>().To<AzureEventSerializer>();
            kernel.Bind<CloudTableClient>().ToConstant(tableClient);
            kernel.Bind<IContactRepository>().ToConstructor(c => new AzureContactRepository(tableClient, tableNameFormatter.FormatTableNam("Contacts")));
            kernel.Bind<IEventStoreRepository>().ToConstructor(c => new AzureEventStoreRepository(tableClient, tableNameFormatter.FormatTableNam("Events"), kernel.Get<IAzureEventSerializer>()));
        }
    }
}