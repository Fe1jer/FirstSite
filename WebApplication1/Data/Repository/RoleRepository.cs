using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Models;
using WebApplication1.Data.Specifications.Base;

namespace WebApplication1.Data.Repository
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(AppDBContext appDBContext) : base(appDBContext)
        {

        }
        public async Task<Role> GetRoleAsync(int id)
        {
            return await GetByIdAsync(id);
        }

        public async Task<Role> GetRoleAsync(string name)
        {
            var users = await GetAllAsync();
            return users.FirstOrDefault(u => u.Name == name);
        }

        public async Task<IReadOnlyList<Role>> GetRolesAsync()
        {
            return await GetAllAsync();
        }

        public async Task<IReadOnlyList<Role>> GetRolesAsync(ISpecification<Role> specification)
        {
            return await GetAllAsync(specification);
        }
    }
}
