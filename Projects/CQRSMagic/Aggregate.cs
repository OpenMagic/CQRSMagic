using System;

namespace CQRSMagic
{
    public class Aggregate : IAggregate
    {
        public Guid Id { get; private set; }
    }
}