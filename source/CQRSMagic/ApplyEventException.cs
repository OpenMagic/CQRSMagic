using System;

namespace CQRSMagic
{
    public class ApplyEventException : Exception
    {
        public ApplyEventException(IEntity entity, IEvent @event)
            : base(CreateMessage(entity, @event))
        {
        }

        private static string CreateMessage(IEntity entity, IEvent @event)
        {
            return string.Format("{0} requires an ApplyEvent({1} @event) method.", entity.GetType().Name, @event.GetType().Name);
        }
    }
}