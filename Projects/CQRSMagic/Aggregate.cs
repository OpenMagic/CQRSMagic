using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Library.CQRS
{
    public class Aggregate : IAggregate
    {
        public Task SendEvents(IEnumerable<IEvent> events)
        {
            return Task.Factory.StartNew(() =>
            {
                var handleEventMethod = GetType().GetMethod("SendEvent", BindingFlags.Instance | BindingFlags.NonPublic);

                foreach (var e in events)
                {
                    var genericHandleEventMethod = handleEventMethod.MakeGenericMethod(e.GetType());

                    genericHandleEventMethod.Invoke(this, new object[] {e});
                }
            });
        }

        // todo: why can't SendEvents find this method when it is private?
        protected internal void SendEvent<TEvent>(TEvent e)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            var handler = this as IHandleEvent<TEvent>;

            if (handler == null)
            {
                throw new Exception(string.Format("{0} does not know how to apply {1} event.", GetType().Name, e.GetType().Name));
            }

            handler.HandleEvent(e);
        }
    }
}