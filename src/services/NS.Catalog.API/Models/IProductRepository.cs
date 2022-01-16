using NS.Core.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NS.Catalog.API.Models
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<PagedResult<Product>> GetAll(int pageSize, int pageIndex, string query = null);
        Task<Product> GetById(Guid id);
        Task<List<Product>> GetProductsById(string ids);

        void Add(Product product);
        void Update(Product product);
        void Save(Product product);
    }
}
