using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetShop.Data.Interfaces;
using InternetShop.Data.Models;
using InternetShop.Data.Repository.Base;

namespace InternetShop.Data.Repository
{
    public class AttributeValueRepository : Repository<AttributeValue>, IAttributeValueRepository
    {
        public AttributeValueRepository(AppDBContext appDBContext) : base(appDBContext) { }

        public new async Task<IReadOnlyList<AttributeValue>> GetAllAsync()
        {
            return await base.GetAllAsync();
        }

        public new async Task<AttributeValue> GetByIdAsync(int productId)
        {
            var products = await GetAllAsync();
            return products.FirstOrDefault(u => u.Id == productId);
        }

        public new async Task AddAsync(AttributeValue productAttribute)
        {
            await base.AddAsync(productAttribute);
        }

        public async Task DeleteListAsync(List<AttributeValue> productAttributes)
        {
            foreach (AttributeValue productAttribute in productAttributes)
            {
                await DeleteAsync(productAttribute);
            }
        }
    }
}
