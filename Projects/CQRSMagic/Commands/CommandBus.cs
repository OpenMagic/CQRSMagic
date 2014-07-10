using System;
using System.Collections.Generic;
using CQRSMagic.Events.Messaging;
using CQRSMagic.Exceptions;

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

            try
            {
                var events = commandHandler(command);

                return events;
            }
            catch (Exception exception)
            {
                var message = string.Format("Cannot send {0} command for {1} with {2}.{3}", command.GetType(), command.AggregateId, commandHandler.Method.DeclaringType, commandHandler.Method.Name);
                throw new CommandException(message, exception);
            }
        }
    }
}