using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Library.CQRS.Support;

namespace Library.CQRS
{
    public class CommandHandlers : ICommandHandlers
    {
        private readonly Lazy<Dictionary<Type, ICommandHandler>> Cache;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CommandHandlers" /> class.
        /// </summary>
        /// <param name="commandHandlerAssemblies">The assemblies that contain types that implement of IHandleCommand&lt;&gt;</param>
        public CommandHandlers(IEnumerable<Assembly> commandHandlerAssemblies)
        {
            Cache = new Lazy<Dictionary<Type, ICommandHandler>>(() => GetCommandHandlers(commandHandlerAssemblies));
        }

        public ICommandHandler GetCommandHandler(ICommand command)
        {
            ICommandHandler commandHandler;

            if (Cache.Value.TryGetValue(command.GetType(), out commandHandler))
            {
                return commandHandler;
            }

            throw new Exception(string.Format("Cannot find command handler for {0} command.", command.GetType()));
        }

        private static Dictionary<Type, ICommandHandler> GetCommandHandlers(IEnumerable<Assembly> commandHandlerAssemblies)
        {
            var types =
                from assembly in commandHandlerAssemblies
                from type in assembly.GetTypes()
                select type;

            var commandHandlers =
                from t in types
                where !t.IsAbstract && !t.IsGenericType && typeof(IAggregateCommandHandlers).IsAssignableFrom(t)
                let obj = (IAggregateCommandHandlers) Activator.CreateInstance(t)
                from i in t.GetInterfaces()
                where i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IHandleCommand<>)
                let genericArguments = i.GetGenericArguments()
                select (ICommandHandler) new CommandHandler(obj, i, genericArguments[0], obj.AggregateType);

            return commandHandlers.ToDictionary(x => x.CommandType);
        }
    }
}