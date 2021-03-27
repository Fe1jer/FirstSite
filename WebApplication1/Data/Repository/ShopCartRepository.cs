using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Models;
using WebApplication1.Data.Specifications;
using WebApplication1.Data.Specifications.Base;

namespace WebApplication1.Data.Repository
{
    public class ShopCartRepository : Repository<ShopCartItem>, IShopCart
    {
        public ShopCartRepository(AppDBContext appDBContext) : base(appDBContext)
        {

        }

        public async Task AddToCart(User user, Product product)
        {
            ShopCartItem shopCartItem = new ShopCartItem
            {
                User = user,
                Product = product,
                Price = product.Price,
                Category = product.Category
            };
            await AddAsync(shopCartItem);
        }

        public async Task RemoveToCart(User user, int id)
        {
            ShopCartItem order = GetAllAsync(new ShopCartSpecification().IncludeProduct().WhereUser(user).WhereProduct(id)).Result.FirstOrDefault();
            await DeleteAsync(order);
        }

        public async Task RemoveToCart(ShopCartItem product)
        {
            await DeleteAsync(product);
        }

        public async Task EmptyTheCart(User user)
        {
            IEnumerable<ShopCartItem> items = await GetShopItemsAsync(new ShopCartSpecification().WhereUser(user));
            foreach (ShopCartItem item in items)
            {
                await DeleteAsync(item);
            }
        }

        public async Task<IReadOnlyList<ShopCartItem>> GetShopItemsAsync()
        {
            return await GetAllAsync();
        }

        public async Task<IReadOnlyList<ShopCartItem>> GetShopItemsAsync(ISpecification<ShopCartItem> specification)
        {
            return await GetAllAsync(specification);
        }
    }
}
