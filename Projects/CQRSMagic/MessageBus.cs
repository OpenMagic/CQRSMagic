using System.Collections;
using System.Collections.Generic;
using OpenMagic.Exceptions;

namespace CQRSMagic
{
    public class MessageBus : IMessageBus
    {
        private readonly IEventStore EventStore;

        public MessageBus(IEventStore eventStore)
        {
            EventStore = eventStore;
        }

        public IEnumerable<IEvent> Send(ICommand command)
        {
            throw new ToDoException();
        }
    }
}