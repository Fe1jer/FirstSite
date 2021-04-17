using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Data.Models;
using WebApplication1.Data.Specifications.Base;
using WebApplication1.ViewModels;

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
        List<FilterCategoryVM> GetFilterCategoriesByProducts(List<Product> products);
        List<Product> SortProducts(List<Product> products, List<string> filters);
        List<ShowProductViewModel> FindProductsInTheCart(List<Product> products, List<ShopCartItem> cartItems);
        List<ShowProductViewModel> DeleteIfInCart(List<Product> products, List<ShopCartItem> cartItems);
    }
}
