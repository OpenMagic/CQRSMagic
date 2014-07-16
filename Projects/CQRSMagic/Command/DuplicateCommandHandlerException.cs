using System;

namespace CQRSMagic.Command
{
    public class DuplicateCommandHandlerException : Exception
    {
        public DuplicateCommandHandlerException(Type commandType)
            : base(string.Format("Cannot register multiple command handlers for {0} command.", commandType))
        {
        }
    }
}