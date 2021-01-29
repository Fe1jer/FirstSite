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

        public IEnumerable<Product> Products => appDBContent.Product.Include(c => c.Category);

        public IEnumerable<Product> GetFavProducts => appDBContent.Product.Where(p => p.IsFavourite).Select(c => c);

        public Product GetObjectProduct(int carId) => appDBContent.Product.FirstOrDefault(p => p.Id == carId);
    }
}
