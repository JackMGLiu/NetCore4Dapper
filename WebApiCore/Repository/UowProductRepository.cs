using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiCore.Infrastructure.Data;
using WebApiCore.Model;

namespace WebApiCore.Repository
{
    public class UowProductRepository : IProductRepository
    {
        private readonly DapperDbContext _context;

        public UowProductRepository(DapperDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddAsync(Product prod)
        {
            return await _context.AddEntityAsync(prod);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.QueryAsync<Product>();
        }

        public async Task<Product> GetByIDAsync(int id)
        {
            return await _context.QueryByKeyAsync<Product>(id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(Product prod)
        {
            return await _context.ModifyEntityAsync(prod);
        }

        public async Task<IEnumerable<Product>> GetAllListAsync()
        {
            var data = await _context.QueryAsync<Product, Category, Product>("select * from Product as p left join Category as c on p.CategoryId=c.Id", (p, c) =>
            {
                p.Category = c;
                return p;
            }, "CategoryId");
            return data;
        }
    }
}
