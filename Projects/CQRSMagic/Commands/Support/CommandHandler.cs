using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CQRSMagic.Events;

namespace CQRSMagic.Commands.Support
{
    internal class CommandHandler
    {
        private readonly Type ConcreteType;
        public readonly Type CommandType;
        public readonly Func<ICommand, IEnumerable<IEvent>> SendCommand;
        private readonly MethodInfo HandleMethod;

        public CommandHandler(Type concreteType, Type commandHandlerType, Type commandType)
        {
            ConcreteType = concreteType;
            CommandType = commandType;
            SendCommand = ExecuteSendCommand;

            // todo: what if IHandleCommand adds new methods
            HandleMethod = commandHandlerType.GetMethods().Single();
        }

        private IEnumerable<IEvent> ExecuteSendCommand(ICommand command)
        {
            // todo: Use a container to create the instance
            var obj = Activator.CreateInstance(ConcreteType);

            var result = HandleMethod.Invoke(obj, new object[] {command});
            var events = (IEnumerable<IEvent>) result;

            return events;
        }
    }
}