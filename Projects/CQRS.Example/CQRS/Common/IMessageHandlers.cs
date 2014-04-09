using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CQRS.Example.CQRS.Common
{
    public interface IMessageHandlers<in TMessageBase>
    {
        /// <summary>
        ///     Registers a message handler.
        /// </summary>
        /// <typeparam name="TMessageHandlerClass">
        ///     The type the that implements message handler.
        /// </typeparam>
        /// <typeparam name="TMessage">
        ///     The type of message that is handled.
        /// </typeparam>
        void RegisterHandler<TMessageHandlerClass, TMessage>() where TMessage : class, TMessageBase;

        /// <summary>
        ///     Searches for, and registers, all message handlers in <paramref name="types" />.
        /// </summary>
        /// <param name="types">
        ///     The types to search for message handlers.
        /// </param>
        Task RegisterHandlers(IEnumerable<Type> types);
    }
}
