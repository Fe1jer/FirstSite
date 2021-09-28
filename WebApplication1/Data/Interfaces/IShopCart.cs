using System.Collections.Generic;
using System.Threading.Tasks;
using InternetShop.Data.Models;
using InternetShop.Data.Specifications.Base;

namespace InternetShop.Data.Interfaces
{
    public interface IShopCart
    {
        Task AddAsync(string email, Product product);
        Task DeleteAsync(string email, int id);
        Task DeleteAsync(ShopCartItem product);
        Task EmptyTheCart(string email);
        Task<IReadOnlyList<ShopCartItem>> GetAllAsync();
        Task<IReadOnlyList<ShopCartItem>> GetAllAsync(ISpecification<ShopCartItem> specification);
    }
}
