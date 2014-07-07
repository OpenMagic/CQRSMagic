using System;
using System.Collections.Generic;
using Anotar.CommonLogging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

// todo: move to AzureMagic.

namespace CQRSMagic.Azure.Support
{
    public static class AzureStorage
    {
        public const string DevelopmentConnectionString = "UseDevelopmentStorage=true;";

        // todo: thread safe collection
        private static readonly HashSet<string> TableNames = new HashSet<string>();

        public static void DeleteTableIfExists(string connectionString, string tableName)
        {
            // todo: unit tests
            GetTable(connectionString, tableName).DeleteIfExists();
        }

        public static CloudTable GetTable(string connectionString, string tableName)
        {
            // todo: unit tests
            return GetTableClient(connectionString).GetTableReference(tableName);
        }

        public static CloudTableClient GetTableClient(string connectionString)
        {
            // todo: unit tests
            return GetAccount(connectionString).CreateCloudTableClient();
        }

        public static CloudStorageAccount GetAccount(string connectionString)
        {
            // todo: unit tests
            return CloudStorageAccount.Parse(connectionString);
        }

        public static void OnceOnlyCreateTableIfNotExists(this CloudTable cloudTable)
        {
            // todo: unit tests
            if (TableNames.Contains(cloudTable.Name))
            {
                return;
            }

            TableNames.Add(cloudTable.Name);

            LogTo.Debug("Create if not exists {0} table.", cloudTable.Name);
            cloudTable.CreateIfNotExists();
            LogTo.Debug("Created if not exists {0} table .", cloudTable.Name);
        }

        /// <summary>
        ///     Converts a GUID to a partition key.
        /// </summary>
        /// <param name="value">The GUID to convert.</param>
        /// <remarks>
        ///     This method simply uses Guid.ToString(). The method's value is code readability.
        /// </remarks>
        public static string ToPartitionKey(this Guid value)
        {
            return value.ToString();
        }

        public static string ToRowKey(this DateTimeOffset value)
        {
            return value.ToString("O");
        }

        public static string ToRowKey(this int value)
        {
            // int.MaxValue is 2,147,483,647. Therefore pad <value> is with leading 0 to 10 characters.
            return value.ToString("D10");
        }
    }
}