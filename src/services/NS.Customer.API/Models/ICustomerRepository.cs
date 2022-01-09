using NS.Core.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NS.Customer.API.Models
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        void Add(Customer customer);
        void AddAddress(Address address);

        Task<IEnumerable<Customer>> GetAll();
        Task<Customer> GetByCpf(string cpf);
        Task<Address> GetAddressById(Guid customerId);
    }
}
