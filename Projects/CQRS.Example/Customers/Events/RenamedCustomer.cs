using System;
using CQRS.Example.CQRS;
using CQRS.Example.CQRS.Events;
using CQRS.Example.Customers.Commands;

namespace CQRS.Example.Customers.Events
{
    public class RenamedCustomer : IEvent
    {
        public readonly Guid CustomerId;
        public readonly string Name;

        public RenamedCustomer(RenameCustomer command)
        {
            CustomerId = command.CustomerId;
            Name = command.Name;
        }
    }
}