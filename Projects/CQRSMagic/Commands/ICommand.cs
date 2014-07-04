using System;

namespace CQRSMagic
{
    public interface ICommand
    {
        Guid AggregateId { get; set; }
    }
}