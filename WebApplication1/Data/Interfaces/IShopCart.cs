using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data.Models;
using WebApplication1.Data.Specifications.Base;

namespace WebApplication1.Data.Interfaces
{
    public interface IShopCart
    {
        Task AddToCart(User user, Product product);
        Task RemoveToCart(User user, int id);
        Task RemoveToCart(ShopCartItem product);
        Task EmptyTheCart(User user);
        Task<IReadOnlyList<ShopCartItem>> GetShopItemsAsync();
        Task<IReadOnlyList<ShopCartItem>> GetShopItemsAsync(ISpecification<ShopCartItem> specification);
    }
}
