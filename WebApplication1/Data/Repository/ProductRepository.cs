using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Models;
using WebApplication1.Data.Specifications.Base;

namespace WebApplication1.Data.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {

        public ProductRepository(AppDBContext appDBContext) : base(appDBContext)
        {

        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync()
        {
            return await GetAllAsync();
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync(ISpecification<Product> specification)
        {
            return await GetAllAsync(specification);
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            return await GetByIdAsync(productId);
        }

        public async Task DeleteProductAsync(Product product)
        {
            await DeleteAsync(product);
        }

        public async Task UpdateProductAsync(Product product)
        {
            await UpdateAsync(product);
        }

        public async Task<Product> GetProductByNameAsync(string name)
        {
            var collection = await GetAllAsync();
            return collection.FirstOrDefault(n => n.Name.Equals(name));
        }

        public async Task<IReadOnlyList<Product>> SearchProductsAsync(string searchText)
        {
            var collection = await GetAllAsync();
            if (searchText != null)
            {
                collection = collection.Where(i => i.Name.ToLower().Contains(searchText.ToLower())).ToList()
                    .Union(collection.Where(i => i.Company.ToLower().Contains(searchText.ToLower()))).Distinct().ToList();
            }
            return collection;
        }

        public async Task AddProductAsync(Product product)
        {
            await AddAsync(product);
        }
    }
}
