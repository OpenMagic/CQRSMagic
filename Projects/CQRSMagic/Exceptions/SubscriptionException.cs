using System;

namespace CQRSMagic.Exceptions
{
    public class SubscriptionException : CQRSMagicException
    {
        public SubscriptionException(string message)
            : base(message)
        {
        }

        public SubscriptionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}