using System;
using System.Threading.Tasks;
using CQRS.Example.CQRS.Events;
using CQRS.Example.Customers.Events;

namespace CQRS.Example.Customers.Domain
{
    public class CustomerEventHandler :
        IEventHandler<CreatedCustomer>,
        IEventHandler<RenamedCustomer>
    {
        private readonly ICustomerRepository Repository;

        public CustomerEventHandler(ICustomerRepository repository)
        {
            Repository = repository;
        }

        public Task Handle(CreatedCustomer @event)
        {
            return Repository.Add(new Customer(@event));
        }

        public async Task Handle(RenamedCustomer @event)
        {
            var customer = await Repository.GetById(@event.CustomerId);

            customer.Name = @event.Name;
        }
    }
}