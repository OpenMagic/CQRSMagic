using System;
using CQRSMagic.Commands;

namespace ExampleDomain.Configuration.Commands
{
    public class ClearAll : ICommand
    {
        public Guid AggregateId { get; set; }
    }
}