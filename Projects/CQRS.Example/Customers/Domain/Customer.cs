using System;
using CQRS.Example.Customers.Events;

namespace CQRS.Example.Customers.Domain
{
    public class Customer
    {
        public Customer(CreatedCustomer @event)
        {
            CustomerId = @event.CustomerId;
            Name = @event.Name;
        }

        public Guid CustomerId { get; private set; }
        public string Name { get; set; }
    }
}