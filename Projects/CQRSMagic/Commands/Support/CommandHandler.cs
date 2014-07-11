using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CQRSMagic.Events.Messaging;

namespace CQRSMagic.Commands.Support
{
    internal class CommandHandler
    {
        public readonly Type CommandType;
        private readonly Type ConcreteType;
        private readonly MethodInfo HandleMethod;
        public readonly Func<ICommand, Task<IEnumerable<IEvent>>> SendCommand;

        public CommandHandler(Type concreteType, Type commandHandlerType, Type commandType)
        {
            ConcreteType = concreteType;
            CommandType = commandType;
            SendCommand = ExecuteSendCommand;

            // todo: what if IHandleCommand adds new methods
            HandleMethod = commandHandlerType.GetMethods().Single();
        }

        private Task<IEnumerable<IEvent>> ExecuteSendCommand(ICommand command)
        {
            // todo: Use a container to create the instance
            var obj = Activator.CreateInstance(ConcreteType);

            var result = HandleMethod.Invoke(obj, new object[] {command});
            var events = (Task<IEnumerable<IEvent>>) result;

            return events;
        }
    }
}