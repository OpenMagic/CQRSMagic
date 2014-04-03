using System;

namespace Library.CQRS
{
    public interface ICommand
    {
        Guid AggregateId { get; }
    }
}