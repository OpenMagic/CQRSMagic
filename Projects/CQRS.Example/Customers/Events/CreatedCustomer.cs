using System;
using CQRS.Example.CQRS;
using CQRS.Example.CQRS.Events;
using CQRS.Example.Customers.Commands;

namespace CQRS.Example.Customers.Events
{
    public class CreatedCustomer : IEvent
    {
        public readonly Guid CustomerId;
        public readonly string Name;

        public CreatedCustomer(CreateCustomer command)
        {
            CustomerId = command.CustomerId;
            Name = command.Name;
        }
    }
}