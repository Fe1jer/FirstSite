using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Data.Models;
using WebApplication1.Data.Specifications.Base;
using WebApplication1.ViewModels;

namespace WebApplication1.Data.Interfaces
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task UpdateAsync(User user, ProfileViewModel model);
        Task<User> GetUserAsync(string email);
        Task<User> GetUserAsync(int id);
        Task DeleteAsync(User user);
        Task<IReadOnlyList<User>> GetAllAsync(ISpecification<User> specification);
        Task<IReadOnlyList<User>> GetAllAsync();
        User CreateUser(RegisterViewModel model);
    }
}
