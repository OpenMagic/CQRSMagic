using System;

namespace CQRSMagic.Exceptions
{
    public class EventException : CQRSMagicException
    {
        public EventException(string message)
            : base(message)
        {
        }

        public EventException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}