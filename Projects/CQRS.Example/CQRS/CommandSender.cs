using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            Handlers.Add(typeof(TCommand), Handler<TCommandHandler, TCommand>());
        }

        private Func<ICommand, Task<IEnumerable<IEvent>>> Handler<TCommandHandler, TCommand>() where TCommand : class, ICommand
        {
            var handleMethod = GetHandleMethod<TCommand>();

            return command =>
            {
                var obj = Container.GetInstance<TCommandHandler>();
                var events = handleMethod.Invoke(obj, new object[] {command});

                return (Task<IEnumerable<IEvent>>) events;
            };
        }

        private static MethodInfo GetHandleMethod<TCommand>() where TCommand : class, ICommand
        {
            var handler = typeof(Commands.ICommandHandler<TCommand>);

            // assuming that IHandle<TCommand> has only 1 method.
            return handler.GetMethods().Single();
        }
    }
}