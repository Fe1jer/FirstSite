using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Data.Models;
using WebApplication1.Data.Specifications.Base;
using WebApplication1.ViewModels;

namespace WebApplication1.Data.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetProductAsync(int productId);
        Task<IReadOnlyList<Product>> GetProductsAsync();
        Task<IReadOnlyList<Product>> GetProductsAsync(ISpecification<Product> specification);
        Task<IReadOnlyList<Product>> SearchProductsAsync(string searchText);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task<Product> GetProductAsync(string name);
        Task DeleteProductAsync(int id);
        List<FilterCategoryVM> GetFilterCategoriesByProducts(List<Product> products);
        List<Product> SortProducts(List<Product> products, List<string> filters);
        Task<List<ShowProductViewModel>> FindProductsInTheCart(List<Product> products, string userName);
        Task<ShowProductViewModel> FindProductInTheCart(Product product, string userName);
        Task<List<ShowProductViewModel>> RemoveIfInCart(List<Product> products, string userName);
    }
}
