using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Example.CQRS.Commands;
using CQRS.Example.CQRS.Events;
using Microsoft.Practices.ServiceLocation;

namespace CQRS.Example.CQRS
{
    public class CommandSender : ICommandSender
    {
        private readonly IServiceLocator Container;
        private readonly Dictionary<Type, Func<ICommand, Task<IEnumerable<IEvent>>>> Handlers = new Dictionary<Type, Func<ICommand, Task<IEnumerable<IEvent>>>>();

        public CommandSender(IServiceLocator container)
        {
            Container = container;
        }

        public Task<IEnumerable<IEvent>> SendCommand(ICommand command)
        {
            var handler = Handlers[command.GetType()];
            var events = handler(command);

            return events;
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
                select new {CommandHandlerClass = type, CommandHandlerInterface = @interface, CommandType = @interface.GetGenericArguments().Single()};

            Parallel.ForEach(commandHandlers, x => RegisterHandler(x.CommandHandlerClass, x.CommandHandlerInterface, x.CommandType));
        }

        private void RegisterHandler(Type commandHandlerClass, Type commandHandlerInterface, Type commandType)
        {
            Handlers.Add(commandType, CreateHandlerFunc(commandHandlerClass, commandHandlerInterface));
        }

        private Func<ICommand, Task<IEnumerable<IEvent>>> CreateHandlerFunc(Type commandHandlerClass, Type commandHandlerInterface)
        {
            // assuming that IHandle<TCommand> has only 1 method.
            var handleMethod = commandHandlerInterface.GetMethods().Single();

            return command =>
            {
                var obj = Container.GetInstance(commandHandlerClass);
                var events = handleMethod.Invoke(obj, new object[] {command});

                return (Task<IEnumerable<IEvent>>) events;
            };
        }
    }
}