using AzureMagic.Tables;
using AzureMagic.Tools;
using CommonServiceLocator.NinjectAdapter.Unofficial;
using CQRSMagic.Command;
using CQRSMagic.EventStorage;
using ExampleDomain.Contacts;
using ExampleDomain.Contacts.Commands;
using ExampleDomain.Repositories.Azure;
using ExampleDomain.Support;
using Microsoft.Practices.ServiceLocation;
using Ninject;

namespace CQRSMagic.Specifications.Support.Configurations
{
    public class AzureConfiguration
    {
        private const string ConnectionString = AzureTableStorage.DevelopmentConnectionString;
        private readonly IKernel Kernel;
        private readonly TableNameFormatter TableNameFormatter;

        public AzureConfiguration(string tableNamePrefix)
        {
            WindowAzureStorageEmulatorManager.StartEmulator();

            TableNameFormatter = new TableNameFormatter(ConnectionString, tableNamePrefix);

            Azure.Settings.Initialize(new[] {typeof(CreateContact).Assembly}, ConnectionString, TableNameFormatter.FormatTableName("Events"));

            Kernel = new StandardKernel();

            ServiceLocator.SetLocatorProvider(() => new NinjectServiceLocator(Kernel));

            // Bindings required by this test project, not CQRSMagic.
            Kernel.Bind<IContactRepository>().ToConstructor(c => new AzureContactRepository(ConnectionString, TableNameFormatter.FormatTableName("Contacts")));
            Kernel.Bind<ICommandBus>().To<CommandBus>();
            Kernel.Bind<IEventStore>().To<EventStore>();
        }

        public T Get<T>()
        {
            return Kernel.Get<T>();
        }

        public void CleanUp()
        {
            TableNameFormatter.DeleteTables();
        }
    }
}