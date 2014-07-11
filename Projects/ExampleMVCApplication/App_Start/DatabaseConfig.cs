using CQRSMagic;
using ExampleDomain.Configuration.Commands;
using ExampleDomain.Contacts.Commands;

namespace ExampleMVCApplication
{
    internal class DatabaseConfig
    {
        public static void InitializeDatabase(IMessageBus messageBus)
        {
            messageBus.SendCommandAsync(new ClearAll());
            messageBus.SendCommandAsync(new AddContact {EmailAddress = "tim@example.com", Name = "Tim"});
            messageBus.SendCommandAsync(new AddContact { EmailAddress = "nicole@example.com", Name = "Nicole" });
        }
    }
}