using System;

namespace CQRSMagic.Exceptions
{
    public abstract class CQRSMagicException : Exception
    {
        protected CQRSMagicException(string message)
            : base(message)
        {
        }

        protected CQRSMagicException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}