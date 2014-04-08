using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Example.CQRS.Events;

namespace CQRS.Example.CQRS.Commands
{
    public class CommandSender : ICommandSender
    {
        private readonly ICommandHandlers CommandHandlers;

        public CommandSender(ICommandHandlers commandHandlers)
        {
            CommandHandlers = commandHandlers;
        }

        public Task<IEnumerable<IEvent>> SendCommand(ICommand command)
        {
            var handler = CommandHandlers.GetHandler(command.GetType());
            var events = handler(command);

            return events;
        }
    }
}