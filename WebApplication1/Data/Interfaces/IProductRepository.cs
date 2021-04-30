using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Data.Models;
using WebApplication1.Data.Specifications.Base;
using WebApplication1.ViewModels;

namespace WebApplication1.Data.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetByIdAsync(int productId);
        Task<IReadOnlyList<Product>> GetAllAsync();
        Task<IReadOnlyList<Product>> GetAllAsync(ISpecification<Product> specification);
        Task<IReadOnlyList<Product>> SearchProductsAsync(string searchText);
        Task AddProductAsync(Product product);
        Task UpdateAsync(Product product);
        Task<Product> GetByNameAsync(string name);
        Task DeleteAsync(int id);
        List<FilterCategoryVM> GetFilterCategoriesByProducts(List<Product> products);
        List<Product> SortProducts(List<Product> products, List<string> filters);
        Task<List<ShowProductViewModel>> FindProductsInTheCart(List<Product> products, string userName);
        Task<ShowProductViewModel> FindProductInTheCart(Product product, string userName);
        Task<List<ShowProductViewModel>> RemoveIfInCart(List<Product> products, string userName);
    }
}
