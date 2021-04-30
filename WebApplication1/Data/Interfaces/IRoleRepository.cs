using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Data.Models;
using WebApplication1.Data.Specifications.Base;

namespace WebApplication1.Data.Interfaces
{
    public interface IRoleRepository
    {
        Task<IReadOnlyList<Role>> GetAllAsync();
        Task<IReadOnlyList<Role>> GetAllAsync(ISpecification<Role> specification);
        Task<Role> GetByIdAsync(int id);
        Task<Role> GetByNameAsync(string name);
    }
}
