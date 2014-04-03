using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CQRSMagic
{
    public class CommandHandler : ICommandHandler
    {
        private readonly MethodInfo Method;
        private readonly object Obj;

        public CommandHandler(object obj, Type @interface, Type commandType, Type aggregateType)
        {
            Obj = obj;
            CommandType = commandType;
            AggregateType = aggregateType;

            // hack: this will fail if IHandleCommand has more than 1 method. Could complete CommandType and AggregateType.
            Method = @interface.GetMethods().Single();
        }

        public Type AggregateType { get; private set; }
        public Type CommandType { get; private set; }

        public Task<IEnumerable<IEvent>> SendCommand(ICommand command, IEventStore eventStore)
        {
            return (Task<IEnumerable<IEvent>>) Method.Invoke(Obj, new object[] {command, eventStore});
        }
    }
}