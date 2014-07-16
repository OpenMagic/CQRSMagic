using System;
using System.Collections.Generic;
using AzureMagic;

namespace CQRSMagic.Specifications.Support
{
    public abstract class TestBase : IDisposable
    {
        private bool IsDisposed;
        private readonly List<Action> CleanupMethods;

        protected string ConnectionString = AzureStorage.DevelopmentConnectionString;
        protected string ContactsTableName = "CQRSMagicSpecificationsContacts" + DateTime.Now.Ticks;

        protected TestBase()
        {
            CleanupMethods = new List<Action>(new Action[]
            {
                DeleteTables
            });
        }

        private void DeleteTables()
        {
            DeleteTable(ContactsTableName);
        }

        private void DeleteTable(string tableName)
        {
            AzureStorage.DeleteTableIfExists(ConnectionString, tableName);
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code.
            // Put cleanup code in Dispose(bool disposing).
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Releases unmanaged and optionally managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
        ///     unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                Cleanup();
            }

            IsDisposed = true;
        }

        protected virtual void Cleanup()
        {
            foreach (var cleanupMethod in CleanupMethods)
            {
                cleanupMethod();
            }
        }
    }
}