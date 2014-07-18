using AzureMagic;
using AzureMagic.Tools;
using CQRSMagic.Azure;
using CQRSMagic.Specifications.Support.IoC;
using ExampleDomain.Contacts;
using ExampleDomain.Contacts.Commands;
using ExampleDomain.Repositories.Azure;
using ExampleDomain.Support;
using Ninject;

namespace CQRSMagic.Specifications.Support.Configurations
{
    public class AzureConfiguration
    {
        private readonly TableNameFormatter TableNameFormatter;
        private readonly IKernel Kernel;
        private const string ConnectionString = AzureStorage.DevelopmentConnectionString;

        public AzureConfiguration(string tableNamePrefix)
        {
            WindowAzureStorageEmulatorManager.StartEmulator();

            TableNameFormatter = new TableNameFormatter(ConnectionString, tableNamePrefix);

            Kernel = new StandardKernel();
            Kernel.Bind<IContactRepository>().ToConstructor(c => new AzureContactRepository(ConnectionString, TableNameFormatter.FormatTableName("Contacts")));

            new DependencyContainer(Kernel).AddAzureBindings(
                connectionString: ConnectionString,
                eventsTableName: TableNameFormatter.FormatTableName("Events"),
                domainAssemblies: new[] {typeof(CreateContact).Assembly});
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
