using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Example.CQRS.Events;
using Microsoft.Practices.ServiceLocation;

namespace CQRS.Example.CQRS.Commands
{
    public class CommandHandlers : ICommandHandlers
    {
        private readonly IServiceLocator Container;
        private readonly Dictionary<Type, Func<ICommand, Task<IEnumerable<IEvent>>>> Handlers;

        public CommandHandlers(IServiceLocator container)
        {
            Container = container;
            Handlers = new Dictionary<Type, Func<ICommand, Task<IEnumerable<IEvent>>>>();
        }

        public Func<ICommand, Task<IEnumerable<IEvent>>> GetHandler(Type commandType)
        {
            Func<ICommand, Task<IEnumerable<IEvent>>> commandHandlerFunc;

            if (!Handlers.TryGetValue(commandType, out commandHandlerFunc))
            {
                throw new Exception(string.Format("Cannot find registered command handler for {0}.", commandType.Name));
            }

            return commandHandlerFunc;
        }

        public void RegisterHandler<TCommandHandler, TCommand>() where TCommand : class, ICommand
        {
            RegisterHandler(typeof(TCommandHandler), typeof(ICommandHandler<TCommand>), typeof(TCommand));
        }

        public Task RegisterHandlers(IEnumerable<Type> types)
        {
            return Task.Factory.StartNew(() => RegisterHandlersSync(types));
        }

        private void RegisterHandlersSync(IEnumerable<Type> types)
        {
            var commandHandlers =
                from type in types
                where type.IsClass && !type.IsAbstract
                from @interface in type.GetInterfaces()
                where @interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(ICommandHandler<>)
                select new { CommandHandlerClass = type, CommandHandlerInterface = @interface, CommandType = @interface.GetGenericArguments().Single() };

            // 9 Apr 2014, Visual Studio 2013. Tempting to use Parallel.ForEach but it seemed to be the cause of NullReferenceException being thrown by RegisterHandler.
            foreach (var commandHandler in commandHandlers)
            {
                RegisterHandler(commandHandler.CommandHandlerClass, commandHandler.CommandHandlerInterface, commandHandler.CommandType);
            }
        }

        private void RegisterHandler(Type commandHandlerClass, Type commandHandlerInterface, Type commandType)
        {
            var commandHandlerFunc = CreateCommandHandlerFunc(commandHandlerClass, commandHandlerInterface);

            Handlers.Add(commandType, commandHandlerFunc);
        }

        private Func<ICommand, Task<IEnumerable<IEvent>>> CreateCommandHandlerFunc(Type commandHandlerClass, Type commandHandlerInterface)
        {
            // assuming that ICommandHandler<TCommand> has only 1 method.
            var handleMethod = commandHandlerInterface.GetMethods().Single();

            return command =>
            {
                var obj = Container.GetInstance(commandHandlerClass);
                var result = handleMethod.Invoke(obj, new object[] { command });
                var events = (Task<IEnumerable<IEvent>>)result;

                return events;
            };
        }
    }
}