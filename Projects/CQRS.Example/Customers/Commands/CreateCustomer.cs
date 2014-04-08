using System;
using CQRS.Example.CQRS;
using CQRS.Example.CQRS.Commands;

namespace CQRS.Example.Customers.Commands
{
    public class CreateCustomer : ICommand
    {
        public readonly Guid CustomerId;
        public readonly string Name;

        public CreateCustomer(Guid customerId, string name)
        {
            CustomerId = customerId;
            Name = name;
        }
    }
}
