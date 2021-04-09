using System.Collections.Generic;
using WebApplication1.Data.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Data.Interfaces
{
    public interface IProductsManager
    {
        string Name { get; }
        List<FilterCategoryVM> GetFilterCategoriesByProducts(List<Product> products);
        List<Product> SortProducts(List<Product> products, List<string> filters);
        List<ShowProductViewModel> FindProductsInTheCart(List<Product> products, List<ShopCartItem> cartItems);
    }
}
