using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CQRS.Example.CQRS.Common;
using CQRS.Example.CQRS.Events;
using Microsoft.Practices.ServiceLocation;

namespace CQRS.Example.CQRS.Commands
{
    public class CommandHandlers : MessageHandlersBase<ICommand, Task<IEnumerable<IEvent>>>, ICommandHandlers
    {
        public CommandHandlers(IServiceLocator container)
            : base(container)
        {
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

        protected override Type GetMessageHandlerType()
        {
            return typeof(ICommandHandler<>);
        }

        protected override Type GetMessageHandlerType<TMessage>()
        {
            return typeof(ICommandHandler<TMessage>);
        }
    }
}