using System;
using System.Collections.Generic;
using Anotar.CommonLogging;
using Microsoft.WindowsAzure.Storage.Table;
using NullGuard;

namespace ExampleDomain.Support
{
    public class TableNameFormatter : IDisposable
    {
        private readonly CloudTableClient TableClient;
        private readonly string TableNameFormat;
        private readonly List<string> TableNames;
        private readonly bool UsingTemporaryTableNames;

        private bool IsDisposed;

        public TableNameFormatter(CloudTableClient tableClient, [AllowNull] string tableNamePrefix)
        {
            UsingTemporaryTableNames = tableNamePrefix != null;
            TableClient = tableClient;
            TableNameFormat = UsingTemporaryTableNames ? tableNamePrefix + "{0}" + DateTime.Now.ToString("yyyyMMddHHmmssfff") : "{0}";
            TableNames = new List<string>();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public string FormatTableNam(string tableName)
        {
            var formattedTableName = string.Format(TableNameFormat, tableName);

            if (!TableNames.Contains(formattedTableName) && UsingTemporaryTableNames)
            {
                LogTo.Trace("New temporary Azure table {0}...", formattedTableName);
                TableNames.Add(formattedTableName);
            }

            return formattedTableName;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed && disposing)
            {
                DeleteTables();
            }

            IsDisposed = true;
        }

        private void DeleteTables()
        {
            foreach (var tableName in TableNames)
            {
                LogTo.Trace("Deleting temporary Azure table {0}...", tableName);
                TableClient.GetTableReference(tableName).DeleteIfExists();
            }
        }
    }
}