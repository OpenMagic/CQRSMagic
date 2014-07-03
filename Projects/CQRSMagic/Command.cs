using System;

namespace CQRSMagic
{
    public class Command : ICommand
    {
        public Guid AggregateId { get; set; }
    }
}