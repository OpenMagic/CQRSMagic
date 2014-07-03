using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Example.Customers.Domain;

namespace CQRS.Example.Customers.Persistence
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IList<Customer> Customers = new List<Customer>();

        public Task Add(Customer customer)
        {
            return Task.Factory.StartNew(() => Customers.Add(customer));
        }

        public Task<IEnumerable<Customer>> GetAll()
        {
            return Task.Factory.StartNew(() => Customers.OrderBy(c => c.Name).AsEnumerable());
        }

        public async Task<Customer> GetById(Guid customerId)
        {
            var customer = await FindById(customerId);

            if (customer == null)
            {
                throw new Exception(string.Format("Cannot find customer {0}.", customerId));
            }

            return customer;
        }

        public Task<Customer> FindById(Guid customerId)
        {
            return Task.Factory.StartNew(() => Customers.SingleOrDefault(c => c.CustomerId == customerId));
        }

        public Task<Customer> FindByName(string customerName)
        {
            return Task.Factory.StartNew(() => Customers.SingleOrDefault(c => c.Name == customerName));
        }
    }
}