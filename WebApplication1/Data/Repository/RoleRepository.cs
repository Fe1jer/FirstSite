using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetShop.Data.Interfaces;
using InternetShop.Data.Models;
using InternetShop.Data.Specifications.Base;

namespace InternetShop.Data.Repository
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(AppDBContext appDBContext) : base(appDBContext)
        {

        }
        public new async Task<Role> GetByIdAsync(int id)
        {
            return await base.GetByIdAsync(id);
        }

        public async Task<Role> GetByNameAsync(string name)
        {
            var users = await base.GetAllAsync();
            return users.FirstOrDefault(u => u.Name == name);
        }

        public new async Task<IReadOnlyList<Role>> GetAllAsync()
        {
            return await base.GetAllAsync();
        }

        public new async Task<IReadOnlyList<Role>> GetAllAsync(ISpecification<Role> specification)
        {
            return await base.GetAllAsync(specification);
        }
    }
}
