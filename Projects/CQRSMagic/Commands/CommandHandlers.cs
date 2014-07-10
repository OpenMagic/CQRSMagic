using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CQRSMagic.Commands.Support;
using CQRSMagic.Events.Messaging;
using CQRSMagic.Exceptions;

namespace CQRSMagic.Commands
{
    public class CommandHandlers : ICommandHandlers
    {
        private readonly Lazy<Dictionary<Type, Func<ICommand, IEnumerable<IEvent>>>> Handlers;

        public CommandHandlers(IEnumerable<Assembly> commandHandlerAssemblies)
        {
            // todo: unit tests
            Handlers = new Lazy<Dictionary<Type, Func<ICommand, IEnumerable<IEvent>>>>(() => FindHandlers(commandHandlerAssemblies));
        }

        public Func<ICommand, IEnumerable<IEvent>> GetCommandHandlerFor(ICommand command)
        {
            // todo: unit tests
            Func<ICommand, IEnumerable<IEvent>> handler;

            if (Handlers.Value.TryGetValue(command.GetType(), out handler))
            {
                return handler;
            }

            throw new CommandException(string.Format("Cannot find handler for {0} command.", command.GetType()));
        }

        internal static Dictionary<Type, Func<ICommand, IEnumerable<IEvent>>> FindHandlers(IEnumerable<Assembly> commandHandlerAssemblies)
        {
            var allTypes =
                from assembly in commandHandlerAssemblies
                from type in assembly.GetTypes()
                select type;

            var commandHandlers =
                from type in allTypes
                from @interface in type.GetInterfaces()
                where @interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(IHandleCommand<>)
                let commandType = @interface.GetGenericArguments()[0]
                select new CommandHandler(type, @interface, commandType);

            return commandHandlers.ToDictionary(x => x.CommandType, x => x.SendCommand);
        }
    }
}