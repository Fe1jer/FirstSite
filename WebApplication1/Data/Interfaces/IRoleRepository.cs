using System.Collections.Generic;
using System.Threading.Tasks;
using InternetShop.Data.Models;
using InternetShop.Data.Specifications.Base;

namespace InternetShop.Data.Interfaces
{
    public interface IRoleRepository
    {
        Task<IReadOnlyList<Role>> GetAllAsync();
        Task<IReadOnlyList<Role>> GetAllAsync(ISpecification<Role> specification);
        Task<Role> GetByIdAsync(int id);
        Task<Role> GetByNameAsync(string name);
    }
}
