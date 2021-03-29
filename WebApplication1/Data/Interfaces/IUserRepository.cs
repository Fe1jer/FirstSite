using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Data.Models;
using WebApplication1.Data.Specifications.Base;

namespace WebApplication1.Data.Interfaces
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);
        Task UpdateUser(User user);
        Task<User> GetUserAsync(string email);
        Task<User> GetUserAsync(int id);
        Task<IReadOnlyList<User>> GetUsersAsync(ISpecification<User> specification);
        Task<IReadOnlyList<User>> GetUsersAsync();
    }
}
