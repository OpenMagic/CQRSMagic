using System;

namespace CQRSMagic
{
    public interface IAggregateCommandHandlers
    {
        Type AggregateType { get; }
    }
}