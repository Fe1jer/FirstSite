using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetShop.Data.Interfaces;
using InternetShop.Data.Models;
using InternetShop.Data.Repository.Base;

namespace InternetShop.Data.Repository
{
    public class AttributeRepository : Repository<Attribute>, IAttributeRepository
    {
        public AttributeRepository(AppDBContext appDBContext) : base(appDBContext)
        {
        }

        public new async Task<IReadOnlyList<Attribute>> GetAllAsync()
        {
            return await base.GetAllAsync();
        }

        public new async Task<Attribute> GetByIdAsync(int productId)
        {
            var attributes = await GetAllAsync();
            return attributes.FirstOrDefault(u => u.Id == productId);
        }
    }
}
