using Microsoft.EntityFrameworkCore;
using NS.Core.Data;
using NS.Customer.API.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NS.Customer.API.Data.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerContext _context;

        public CustomerRepository(CustomerContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public void Add(Models.Customer customer)
        {
            _context.Customers.Add(customer);
        }

        public async Task<IEnumerable<Models.Customer>> GetAll()
        {
            return await _context.Customers.AsNoTracking().ToListAsync();
        }

        public Task<Models.Customer> GetByCpf(string cpf)
        {
            return _context.Customers.FirstOrDefaultAsync(c => c.Cpf.Number == cpf);
        }

        public async Task<Address> GetAddressById(Guid customerId)
        {
            return await _context.Addresses.FirstOrDefaultAsync(ad => ad.CustomerId == customerId);
        }

        public void AddAddress(Address address)
        {
            _context.Addresses.Add(address);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
