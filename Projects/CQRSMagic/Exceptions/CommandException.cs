using System;

namespace CQRSMagic.Exceptions
{
    public class CommandException : CQRSMagicException
    {
        public CommandException(string message)
            : base(message)
        {
        }

        public CommandException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}