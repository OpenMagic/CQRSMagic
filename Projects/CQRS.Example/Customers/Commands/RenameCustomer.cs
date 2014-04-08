using System;
using CQRS.Example.CQRS;
using CQRS.Example.CQRS.Commands;

namespace CQRS.Example.Customers.Commands
{
    public class RenameCustomer : ICommand
    {
        public readonly Guid CustomerId;
        public readonly string Name;

        public RenameCustomer(Guid customerId, string name)
        {
            CustomerId = customerId;
            Name = name;
        }
    }
}