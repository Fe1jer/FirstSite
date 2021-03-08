using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data.Models;

namespace WebApplication1.Data.Interfaces
{
    public interface IShopCart
    {
        public void AddToCart(User user, Product product);
        public void RemoveToCart(User user, int id);
        public void EmptyTheCart(User user);
        public List<ShopCartItem> GetShopItems(User user);
    }
}
