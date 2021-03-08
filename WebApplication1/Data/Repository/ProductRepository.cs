using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Models;

namespace WebApplication1.Data.Repository
{
    public class ProductRepository : IAllProduct
    {
        private readonly AppDBContext appDBContent;

        public ProductRepository(AppDBContext appDBContent)
        {
            this.appDBContent = appDBContent;
        }

        public IEnumerable<Product> Products => appDBContent.Product;

        public IEnumerable<Product> GetFavProducts => appDBContent.Product.Where(p => p.IsFavourite).Select(c => c);

        public Product GetObjectProduct(int productId) => appDBContent.Product.FirstOrDefault(p => p.Id == productId);

        public void DeleteProduct(int productId) {
            Product product = appDBContent.Product.FirstOrDefault(p => p.Id == productId);
            appDBContent.Product.Remove(product);
            List<ShopCartItem> deleteProducts = appDBContent.ShopCartItem.Where(p=>p.Product==product).ToList();
            foreach(ShopCartItem p in deleteProducts)
            {
                appDBContent.ShopCartItem.Remove(p);
            }
            
            appDBContent.SaveChanges(); 
        }

        public void ChangeProduct(Product obj) {
            Product product = appDBContent.Product.FirstOrDefault(p => p.Id == obj.Id);

            if(product != null)
            {
                product.Name = obj.Name;
                product.ShortDesc = obj.ShortDesc;
                product.LongDesc = obj.LongDesc;
                product.Category = obj.Category;
                product.Company = obj.Company;
                product.Country = obj.Country;
                product.Price = obj.Price;
                product.Img = obj.Img;
                product.IsFavourite = obj.IsFavourite;
                product.Available = obj.Available;
            }

            appDBContent.SaveChanges(); 
        }

        public void CreateProduct(Product product) {
            appDBContent.Product.Add(product);
            appDBContent.SaveChanges(); 
        }
    }
}
