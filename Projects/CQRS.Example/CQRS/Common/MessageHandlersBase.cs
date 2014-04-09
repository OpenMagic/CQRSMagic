using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;

namespace CQRS.Example.CQRS.Common
{
    public abstract class MessageHandlersBase<TMessageBase, TReturn> : IMessageHandlers<TMessageBase>
    {
        private readonly IServiceLocator Container;

        protected MessageHandlersBase(IServiceLocator container)
        {
            Container = container;
        }

        public void RegisterHandler<TMessageHandlerClass, TMessage>() where TMessage : class, TMessageBase
        {
            RegisterHandler(typeof(TMessageHandlerClass), GetMessageHandlerType<TMessage>(), typeof(TMessage));
        }

        public Task RegisterHandlers(IEnumerable<Type> types)
        {
            return Task.Factory.StartNew(() => RegisterHandlersSync(types));
        }

        protected abstract void AddHandler(Type messageType, Func<TMessageBase, TReturn> messageHandlerFunc);

        protected abstract Type GetMessageHandlerType();

        protected abstract Type GetMessageHandlerType<TMessage>() where TMessage : TMessageBase;

        protected void RegisterHandler(Type messageHandlerClass, Type messageHandlerInterface, Type messageType)
        {
            var messageHandlerFunc = CreateMessageHandlerFunc(messageHandlerClass, messageHandlerInterface);

            AddHandler(messageType, messageHandlerFunc);
        }

        private void RegisterHandlersSync(IEnumerable<Type> types)
        {
            var messageHandlers =
                from type in types
                where type.IsClass && !type.IsAbstract
                from @interface in type.GetInterfaces()
                where @interface.IsGenericType && @interface.GetGenericTypeDefinition() == GetMessageHandlerType()
                select new {MessageHandlerClass = type, MessageHandlerInterface = @interface, MessageType = @interface.GetGenericArguments().Single()};

            // 9 Apr 2014, Visual Studio 2013. Tempting to use Parallel.ForEach but it seemed to be the cause of NullReferenceException being thrown by RegisterHandler.
            foreach (var messageHandler in messageHandlers)
            {
                RegisterHandler(messageHandler.MessageHandlerClass, messageHandler.MessageHandlerInterface, messageHandler.MessageType);
            }
        }

        private Func<TMessageBase, TReturn> CreateMessageHandlerFunc(Type messageHandlerClass, Type messageHandlerInterface)
        {
            // todo: assuming that IMessageHandler<TMessage> has only 1 method.
            var handleMethod = messageHandlerInterface.GetMethods().Single();

            return @message =>
            {
                var obj = Container.GetInstance(messageHandlerClass);
                var result = handleMethod.Invoke(obj, new object[] {@message});
                var cast = (TReturn) result;

                return cast;
            };
        }
    }
}