using System.Collections.Generic;
using CQRSMagic.Events.Messaging;

namespace CQRSMagic.Commands
{
    public class CommandBus : ICommandBus
    {
        private readonly ICommandHandlers CommandHandlers;

        public CommandBus(ICommandHandlers commandHandlers)
        {
            CommandHandlers = commandHandlers;
        }

        public IEnumerable<IEvent> SendCommand(ICommand command)
        {
            // todo: unit tests
            var commandHandler = CommandHandlers.GetCommandHandlerFor(command);
            var events = commandHandler(command);

            return events;
        }
    }
}