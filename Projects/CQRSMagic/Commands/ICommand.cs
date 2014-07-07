using System;

namespace CQRSMagic.Commands
{
    public interface ICommand
    {
        Guid AggregateId { get; set; }
    }
}