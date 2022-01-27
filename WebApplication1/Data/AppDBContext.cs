using Microsoft.EntityFrameworkCore;
using InternetShop.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace InternetShop.Data
{
    public class AppDBContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public AppDBContext(DbContextOptions<AppDBContext> option) : base(option) { }
        public DbSet<Product> Product { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<ShopCartItem> ShopCartItem { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<SiteRating> SiteRating { get; set; }
        public DbSet<AttributeCategory> AttributeCategory { get; set; }
        public DbSet<ProductAttribute> ProductAttribute { get; set; }
    }
}
