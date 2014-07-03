using System;

namespace CQRSMagic
{
    public interface IAggregate
    {
        Guid Id { get; }
    }
}