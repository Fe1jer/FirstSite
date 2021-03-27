using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Data.Models;
using WebApplication1.Data.Specifications.Base;

namespace WebApplication1.Data.Interfaces
{
    public interface IRoleRepository
    {
        Task<IReadOnlyList<Role>> GetRolesAsync();
        Task<IReadOnlyList<Role>> GetRolesAsync(ISpecification<Role> specification);
        Task<Role> GetRoleAsync(int id);
        Task<Role> GetRoleAsync(string name);
    }
}
