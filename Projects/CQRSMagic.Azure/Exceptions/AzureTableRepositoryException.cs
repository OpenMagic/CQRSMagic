using System;

namespace CQRSMagic.Azure.Exceptions
{
    public class AzureTableRepositoryException : CQRSMagicAzureException
    {
        public AzureTableRepositoryException(string message)
            : base(message)
        {
        }

        public AzureTableRepositoryException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}