using System;

namespace CQRSMagic.Command
{
    public interface ICommand
    {
        Guid AggregateId { get; }
    }
}