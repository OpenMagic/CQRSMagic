using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using CQRSMagic.Event;

namespace CQRSMagic.Command
{
    public class CommandHandlers : ICommandHandlers
    {
        private readonly Dictionary<Type, Func<ICommand, Task<IEnumerable<IEvent>>>> Handlers;

        public CommandHandlers()
        {
            Handlers = new Dictionary<Type, Func<ICommand, Task<IEnumerable<IEvent>>>>();
        }

        public void RegisterHandler<TCommand>(Func<TCommand, Task<IEnumerable<IEvent>>> handler) where TCommand : ICommand
        {
            var key = typeof(TCommand);

            if (Handlers.ContainsKey(key))
            {
                throw new DuplicateCommandHandlerException(key);
            }

            Func<ICommand, Task<IEnumerable<IEvent>>> value = command => handler((TCommand)command);

            Handlers.Add(key, value);
        }

        public void RegisterHandlers(Assembly searchAssembly)
        {
            // CommandBus.RegisterHandler<AddContact>(command => Task.FromResult((IEnumerable<IEvent>)new IEvent[] { new ContactAdded(command) }));
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
    }
}