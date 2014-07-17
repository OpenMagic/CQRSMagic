using System;

namespace CQRSMagic.Command
{
    public class CommandBase : ICommand
    {
        public Guid AggregateId { get; set; }
    }
}