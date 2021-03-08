using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Data.Repository
{
    public class ShopCartRepository: IShopCart
    {
        private readonly AppDBContext appDBContent;
        public ShopCartRepository(AppDBContext appDBContent)
        {
            this.appDBContent = appDBContent;
        }

        public void AddToCart(User user, Product product)
        {
            appDBContent.ShopCartItem.Add(new ShopCartItem
            {
                User = user,
                Product = product,
                Price = product.Price,
                Category = product.Category
            });
            appDBContent.SaveChanges();
        }

        public void RemoveToCart(User user, int id)
        {
            ShopCartItem order = appDBContent.ShopCartItem.Include(s => s.Product).Where(o => o.User == user).Where(o => o.Product.Id == id).FirstOrDefault();
            appDBContent.ShopCartItem.Remove(order);
            appDBContent.SaveChanges();
        }

        public void EmptyTheCart(User user)
        {
            List<ShopCartItem> items = appDBContent.ShopCartItem.Where(c => c.User == user).ToList();
            foreach (ShopCartItem item in items)
            {
                appDBContent.ShopCartItem.Remove(item);
            }
            appDBContent.SaveChanges();
        }

        public List<ShopCartItem> GetShopItems(User user)
        {
            return appDBContent.ShopCartItem.Where(c => c.User == user).Include(s => s.Product).ToList();
        }
    }
}
