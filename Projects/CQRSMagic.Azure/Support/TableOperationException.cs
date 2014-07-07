using System;

namespace CQRSMagic.Azure.Support
{
    public class TableOperationException : Exception
    {
        public TableOperationException(string message)
            : base(message)
        {
        }

        public TableOperationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}