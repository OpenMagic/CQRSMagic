using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Anotar.CommonLogging;
using CQRSMagic.Event;
using CQRSMagic.EventStorage;
using CQRSMagic.Support;

namespace CQRSMagic.Command
{
    public class CommandBus : ICommandBus
    {
        private readonly ICommandHandlers CommandHandlers;
        private readonly IEventBus EventBus;

        public CommandBus()
            : this(IoC.Get<ICommandHandlers>(), IoC.Get<IEventBus>())
        {
        }

        public CommandBus(ICommandHandlers commandHandlers, IEventBus eventBus)
        {
            EventBus = eventBus;
            CommandHandlers = commandHandlers;
        }

        public async Task SendCommandAsync(ICommand command)
        {
            var events = (await GetEvents(command)).ToArray();

            await EventBus.SendEventsAsync(events);
        }

        public void RegisterHandler<TCommand>(Func<TCommand, Task<IEnumerable<IEvent>>> handler) where TCommand : ICommand
        {
            CommandHandlers.RegisterHandler(handler);
        }

        public void RegisterHandlers(Assembly searchAssembly)
        {
            CommandHandlers.RegisterHandlers(searchAssembly);
        }

        private Task<IEnumerable<IEvent>> GetEvents(ICommand command)
        {
            var handler = CommandHandlers.GetHandler(command);
            var events = handler(command);

            return events;
        }
    }
}