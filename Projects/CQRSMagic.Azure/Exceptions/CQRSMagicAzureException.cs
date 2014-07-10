using System;
using CQRSMagic.Exceptions;

namespace CQRSMagic.Azure.Exceptions
{
    public abstract class CQRSMagicAzureException : CQRSMagicException
    {
        protected CQRSMagicAzureException(string message)
            : base(message)
        {
        }

        protected CQRSMagicAzureException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}