using System;
using Anotar.CommonLogging;
using CQRSMagic;
using ExampleDomain.Configuration.Commands;
using ExampleDomain.Contacts.Commands;

namespace ExampleMVCApplication
{
    public class DatabaseConfig
    {
        public static void InitializeDatabase(IMessageBus messageBus)
        {
            try
            {
                messageBus.SendCommandAsync(new ClearAll());
                messageBus.SendCommandAsync(new AddContact { EmailAddress = "tim@example.com", Name = "Tim" });
                messageBus.SendCommandAsync(new AddContact { EmailAddress = "nicole@example.com", Name = "Nicole" });
            }
            catch (Exception exception)
            {
                var ex = new Exception("Cannot initialize the database.", exception);
                LogTo.ErrorException(ex.Message, ex);
                throw ex;
            }
        }
    }
}