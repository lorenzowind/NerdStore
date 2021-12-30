using NS.Core.DomainObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NS.Customer.API.Models
{
    public interface ICustomerRepository : IRepository<CustomerPerson>
    {
        void Add(CustomerPerson customerPerson);

        Task<IEnumerable<CustomerPerson>> GetAll();
        Task<CustomerPerson> GetByCpf(string cpf);
    }
}
