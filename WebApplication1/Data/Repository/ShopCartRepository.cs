using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetShop.Data.Interfaces;
using InternetShop.Data.Models;
using InternetShop.Data.Specifications;
using InternetShop.Data.Specifications.Base;
using Microsoft.AspNetCore.Identity;

namespace InternetShop.Data.Repository
{
    public class ShopCartRepository : Repository<ShopCartItem>, IShopCart
    {
        private readonly UserManager<User> _userManager;

        public ShopCartRepository(AppDBContext appDBContext, UserManager<User> userManager) : base(appDBContext)
        {
            _userManager = userManager;
        }

        public async Task AddAsync(string email, Product product)
        {
            User user = await _userManager.FindByEmailAsync(email);
            ShopCartItem shopCartItem = new ShopCartItem
            {
                User = user,
                Product = product,
                Price = product.Price,
                Category = product.Category
            };
            await AddAsync(shopCartItem);
        }

        public async Task DeleteAsync(string email, int id)
        {
            ShopCartItem order = base.GetAllAsync(new ShopCartSpecification().WhereUserEmail(email).WhereProduct(id)).Result.FirstOrDefault();
            await base.DeleteAsync(order);
        }

        public new async Task DeleteAsync(ShopCartItem product)
        {
            await base.DeleteAsync(product);
        }

        public async Task EmptyTheCart(string email)
        {
            User user = await _userManager.FindByEmailAsync(email);
            IEnumerable<ShopCartItem> items = await GetAllAsync(new ShopCartSpecification().WhereUserEmail(user.Email));
            foreach (ShopCartItem item in items)
            {
                await base.DeleteAsync(item);
            }
        }

        public new async Task<IReadOnlyList<ShopCartItem>> GetAllAsync()
        {
            return await base.GetAllAsync();
        }

        public new async Task<IReadOnlyList<ShopCartItem>> GetAllAsync(ISpecification<ShopCartItem> specification)
        {
            return await base.GetAllAsync(specification);
        }
    }
}
