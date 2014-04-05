using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CQRSMagic.Support
{
    public class CommandHandler : ICommandHandler
    {
        private readonly IAggregateCommandHandlers AggregateCommandHandlers;
        private readonly MethodInfo HandleCommandMethod;

        public CommandHandler(IAggregateCommandHandlers aggregateCommandHandlers, Type commandInterface, Type commandType, Type aggregateType)
        {
            AggregateCommandHandlers = aggregateCommandHandlers;
            CommandType = commandType;
            AggregateType = aggregateType;
            HandleCommandMethod = GetHandleCommandMethod(commandInterface);
        }

        private static MethodInfo GetHandleCommandMethod(Type commandInterface)
        {
            // ReSharper disable RedundantCast
            var handleCommandMethodInfo = StaticReflection.GetMethodInfo<IHandleCommand<ICommand>>(x => x.HandleCommand((ICommand)null, (IEventStore)null));
            // ReSharper restore RedundantCast
            var handleCommandParameters = handleCommandMethodInfo.GetParameters();

            return commandInterface.GetMethods().Single(m =>
            {
                if (m.Name != handleCommandMethodInfo.Name)
                {
                    return false;
                }

                var parameters = m.GetParameters();

                if (parameters.Length != handleCommandParameters.Length)
                {
                    return false;
                }

                var matchingParameters = parameters.Select((p, i) => handleCommandParameters[i].ParameterType.IsAssignableFrom(p.ParameterType));

                return matchingParameters.All(b => b);
            });
        }

        public Type AggregateType { get; private set; }
        public Type CommandType { get; private set; }

        public Task<IEnumerable<IEvent>> SendCommand(ICommand command, IEventStore eventStore)
        {
            return (Task<IEnumerable<IEvent>>)HandleCommandMethod.Invoke(AggregateCommandHandlers, new object[] { command, eventStore });
        }
    }
}