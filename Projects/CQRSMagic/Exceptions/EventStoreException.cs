using System;

namespace CQRSMagic.Exceptions
{
    public class EventStoreException : CQRSMagicException
    {
        public EventStoreException(string message)
            : base(message)
        {
        }

        public EventStoreException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}