using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Data.Models;
using WebApplication1.Data.Specifications.Base;

namespace WebApplication1.Data.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetProductByIdAsync(int productId);
        Task<IReadOnlyList<Product>> GetProductsAsync();
        Task<IReadOnlyList<Product>> GetProductsAsync(ISpecification<Product> specification);
        Task<IReadOnlyList<Product>> SearchProductsAsync(string searchText);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task<Product> GetProductByNameAsync(string name);
        Task DeleteProductAsync(Product product);
    }
}
