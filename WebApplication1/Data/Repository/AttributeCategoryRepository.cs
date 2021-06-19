using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Models;
using WebApplication1.Data.Specifications.Base;

namespace WebApplication1.Data.Repository
{
    public class AttributeCategoryRepository : Repository<AttributeCategory>, IAttributeCategoryRepository
    {
        public AttributeCategoryRepository(AppDBContext appDBContext) : base(appDBContext)
        {
        }

        public new async Task<IReadOnlyList<AttributeCategory>> GetAllAsync()
        {
            return await base.GetAllAsync();
        }

        public new async Task<AttributeCategory> GetByIdAsync(int productId)
        {
            var products = await GetAllAsync();
            return products.FirstOrDefault(u => u.Id == productId);
        }
    }
}
