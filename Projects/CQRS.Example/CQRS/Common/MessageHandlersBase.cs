using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;

namespace CQRS.Example.CQRS.Common
{
    public abstract class MessageHandlersBase<TMessageHandler, TMessageBase, TMessageReturn> : IMessageHandlers<TMessageBase>
        where TMessageHandler : IMessageHandler<TMessageBase>
    {
        private readonly IServiceLocator Container;
        protected readonly Dictionary<Type, Func<TMessageBase, TMessageReturn>> Handlers;

        protected MessageHandlersBase(IServiceLocator container)
        {
            Container = container;
            Handlers = new Dictionary<Type, Func<TMessageBase, TMessageReturn>>();
        }

        public void RegisterHandler<TMessageHandlerClass, TMessage>()
            where TMessage : class, TMessageBase
            where TMessageHandlerClass : class, IMessageHandler<TMessage>
        {
            RegisterHandler(typeof(TMessageHandlerClass), GetMessageHandlerType<TMessage>(), typeof(TMessage));
        }

        public Task RegisterHandlers(IEnumerable<Type> types)
        {
            return Task.Factory.StartNew(() => RegisterHandlersSync(types));
        }

        protected void RegisterHandler(Type messageHandlerClass, Type messageHandlerInterface, Type messageType)
        {
            var messageHandlerFunc = CreateMessageHandlerFunc(messageHandlerClass, messageHandlerInterface);

            Handlers.Add(messageType, messageHandlerFunc);
        }

        private void RegisterHandlersSync(IEnumerable<Type> types)
        {
            var messageHandlerType = GetMessageHandlerType();

            var messageHandlers =
                from type in types
                where type.IsClass && !type.IsAbstract
                from @interface in type.GetInterfaces()
                where @interface.IsGenericType && @interface.GetGenericTypeDefinition() == messageHandlerType
                select new {MessageHandlerClass = type, MessageHandlerInterface = @interface, MessageType = @interface.GetGenericArguments().Single()};

            // 9 Apr 2014, Visual Studio 2013. Tempting to use Parallel.ForEach but it seemed to be the cause of NullReferenceException being thrown by RegisterHandler.
            foreach (var messageHandler in messageHandlers)
            {
                RegisterHandler(messageHandler.MessageHandlerClass, messageHandler.MessageHandlerInterface, messageHandler.MessageType);
            }
        }

        private Type GetMessageHandlerType()
        {
            var type = typeof(TMessageHandler);
            var genericType = type.GetGenericTypeDefinition();

            return genericType;
        }

        private Type GetMessageHandlerType<TMessage>()
        {
            var genericType = GetMessageHandlerType();
            var type = genericType.MakeGenericType(new[] {typeof(TMessage)});

            return type;
        }

        private Func<TMessageBase, TMessageReturn> CreateMessageHandlerFunc(Type messageHandlerClass, Type messageHandlerInterface)
        {
            // todo: assuming that IMessageHandler<TMessage> has only 1 method.
            var handleMethod = messageHandlerInterface.GetMethods().Single();

            return @message =>
            {
                var obj = Container.GetInstance(messageHandlerClass);
                var result = handleMethod.Invoke(obj, new object[] {@message});
                var cast = (TMessageReturn) result;

                return cast;
            };
        }
    }
}