using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiCore.Model;

namespace WebApiCore.Repository
{
    public interface IProductRepository
    {
        Task<bool> AddAsync(Product prod);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIDAsync(int id);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateAsync(Product prod);
    }
}