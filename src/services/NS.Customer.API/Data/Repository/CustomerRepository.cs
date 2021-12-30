using Microsoft.EntityFrameworkCore;
using NS.Core.Data;
using NS.Customer.API.Models;
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

        public void Add(CustomerPerson customer)
        {
            _context.Customers.Add(customer);
        }

        public async Task<IEnumerable<CustomerPerson>> GetAll()
        {
            return await _context.Customers.AsNoTracking().ToListAsync();
        }

        public Task<CustomerPerson> GetByCpf(string cpf)
        {
            return _context.Customers.FirstOrDefaultAsync(c => c.Cpf.Number == cpf);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
