using System;

namespace CQRSMagic.Command
{
    public class CommandHandlerNotFoundException : Exception
    {
        public CommandHandlerNotFoundException(ICommand command)
            : base(string.Format("Cannot find command handler for {0} command.", command.GetType()))
        {
        }
    }
}