using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Example.CQRS.Commands;
using CQRS.Example.CQRS.Events;
using CQRS.Example.Customers.Commands;
using CQRS.Example.Customers.Events;

namespace CQRS.Example.Customers.Domain
{
    public class CustomerCommandHandler : ICommandHandler<CreateCustomer>, ICommandHandler<RenameCustomer>
    {
        private readonly ICustomerRepository Repository;

        public CustomerCommandHandler(ICustomerRepository repository)
        {
            Repository = repository;
        }

        public Task<IEnumerable<IEvent>> Handle(CreateCustomer command)
        {
            return Task.Factory.StartNew(() => new IEvent[] {
                new CreatedCustomer(command)
            }.AsEnumerable());
        }

        public async Task<IEnumerable<IEvent>> Handle(RenameCustomer command)
        {
            // todo: can this somehow be handled with yield return?
            var events = new List<IEvent>();
            var customer = await Repository.GetById(command.CustomerId);

            if (customer.Name != command.Name)
            {
                events.Add(new RenamedCustomer(command));
            }

            return events.AsEnumerable();
        }
    }
}