using System.Collections.Generic;
using CQRSMagic.Commands;

namespace CQRSMagic.Specifications.Support
{
    public static class MessageBusExtensions
    {
        public static void SendCommands(this IMessageBus messageBus, IEnumerable<ICommand> commands)
        {
            foreach (var command in commands)
            {
                messageBus.SendCommandAsync(command).Wait();
            }
        }
    }
}
