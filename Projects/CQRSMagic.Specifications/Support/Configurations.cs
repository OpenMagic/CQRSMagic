using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureMagic;
using AzureMagic.Tools;
using CQRSMagic.Azure;
using CQRSMagic.IoC;
using ExampleDomain.Contacts;
using ExampleDomain.Contacts.Commands;
using ExampleDomain.Repositories.Azure;
using ExampleDomain.Support;
using Ninject;

namespace CQRSMagic.Specifications.Support
{
    public static class Configurations
    {
        private static TableNameFormatter TableNameFormatter;

        public static BasicIoC Azure(string tableNamePrefix)
        {
            WindowAzureStorageEmulatorManager.StartEmulator();

            const string connectionString = AzureStorage.DevelopmentConnectionString;

            TableNameFormatter = new TableNameFormatter(connectionString, tableNamePrefix);

            var kernel = new StandardKernel();
            kernel.Bind<IContactRepository>().ToConstructor(c => new AzureContactRepository(connectionString, TableNameFormatter.FormatTableName("Contacts")));

            var container = new CQRSAzureContainer(
                connectionString: connectionString,
                eventsTableName: TableNameFormatter.FormatTableName("Events"),
                domainAssemblies: new[] { typeof(CreateContact).Assembly },
                unboundTypeFactory: type => kernel.Get(type));

            return container;
        }

        public static void DeleteAzureTables()
        {
            if (TableNameFormatter == null)
            {
                return;
            }
            TableNameFormatter.DeleteTables();
        }
    }
}
