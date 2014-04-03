using System;

namespace Library.CQRS
{
    public interface IAggregateCommandHandlers
    {
        Type AggregateType { get; }
    }
}