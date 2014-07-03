using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CQRS.Example.Customers.Domain
{
    public interface ICustomerRepository
    {
        Task Add(Customer customer);
        Task<IEnumerable<Customer>> GetAll();
        Task<Customer> GetById(Guid customerId);
        Task<Customer> FindById(Guid customerId);
        Task<Customer> FindByName(string customerName);
    }
}