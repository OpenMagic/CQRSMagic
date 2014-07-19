﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CQRSMagic.Event;
using CQRSMagic.EventStorage;
using CQRSMagic.Support;

namespace CQRSMagic.Command
{
    public class CommandBus : ICommandBus
    {
        private readonly ICommandHandlers CommandHandlers;
        private readonly IEventBus EventBus;
        private readonly IEventStore EventStore;

        public CommandBus()
            : this(IoC.Get<IEventStore>(), IoC.Get<IEventBus>(), IoC.Get<ICommandHandlers>())
        {
        }

        public CommandBus(IEventStore eventStore, IEventBus eventBus, ICommandHandlers commandHandlers)
        {
            EventStore = eventStore;
            EventBus = eventBus;
            CommandHandlers = commandHandlers;
        }

        public async Task<IEnumerable<Task>> SendCommandAsync(ICommand command)
        {
            var tasks = new List<Task>();
            var events = (await GetEvents(command)).ToArray();

            tasks.Add(EventStore.SaveEventsAsync(events));
            tasks.Add(EventBus.SendEventsAsync(events));

            return tasks.AsEnumerable();
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