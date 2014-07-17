using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CQRSMagic.Event;

namespace CQRSMagic.Command
{
    public class CommandHandlers : ICommandHandlers
    {
        private readonly IDependencyResolver DependencyResolver;
        private readonly Dictionary<Type, Func<ICommand, Task<IEnumerable<IEvent>>>> Handlers;

        public CommandHandlers(IDependencyResolver dependencyResolver)
        {
            DependencyResolver = dependencyResolver;
            Handlers = new Dictionary<Type, Func<ICommand, Task<IEnumerable<IEvent>>>>();
        }

        public Func<ICommand, Task<IEnumerable<IEvent>>> GetHandler(ICommand command)
        {
            Func<ICommand, Task<IEnumerable<IEvent>>> handler;

            if (Handlers.TryGetValue(command.GetType(), out handler))
            {
                return handler;
            }

            throw new CommandHandlerNotFoundException(command);
        }

        public void RegisterHandler<TCommand>(Func<TCommand, Task<IEnumerable<IEvent>>> handler) where TCommand : ICommand
        {
            var key = typeof(TCommand);
            Func<ICommand, Task<IEnumerable<IEvent>>> value = command => handler((TCommand)command);

            RegisterHandler(key, value);
        }

        public void RegisterHandlers(Assembly searchAssembly)
        {
            var commandHandlers =
                from type in searchAssembly.GetTypes()
                from @interface in type.GetInterfaces()
                where @interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(IHandleCommand<>)
                let commandType = @interface.GetGenericArguments()[0]
                select new { commandType, handler = CreateCommandHandler(type, @interface.GetMethods().Single()) };

            foreach (var item in commandHandlers)
            {
                RegisterHandler(item.commandType, item.handler);
            }
        }

        private void RegisterHandler(Type commandType, Func<ICommand, Task<IEnumerable<IEvent>>> handler)
        {
            if (Handlers.ContainsKey(commandType))
            {
                throw new DuplicateCommandHandlerException(commandType);
            }

            Handlers.Add(commandType, handler);
        }

        private Func<ICommand, Task<IEnumerable<IEvent>>> CreateCommandHandler(Type commandHandlerType, MethodInfo commandHandlerMethod)
        {
            Func<ICommand, Task<IEnumerable<IEvent>>> handler = command => HandleCommand(command, commandHandlerType, commandHandlerMethod);

            return handler;
        }

        private Task<IEnumerable<IEvent>> HandleCommand(ICommand command, Type commandHandlerType, MethodInfo commandHandlerMethod)
        {
            var commandHandler = DependencyResolver.GetService(commandHandlerType);
            var result = commandHandlerMethod.Invoke(commandHandler, new object[] { command });
            var task = (Task<IEnumerable<IEvent>>)result;

            return task;
        }
    }
}