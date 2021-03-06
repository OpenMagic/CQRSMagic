﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Anotar.CommonLogging;
using CQRSMagic.Event;
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

        public async Task<Task> SendCommandAsync(ICommand command)
        {
            LogTo.Trace("Sending {0} command.", command.GetType());

            var events = await GetEvents(command);
            var sendEvents = EventBus.SendEventsAsync(events);

            await sendEvents.ContinueWith(c => LogTo.Trace("Sent {0} command.", command.GetType()));

            return sendEvents;
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