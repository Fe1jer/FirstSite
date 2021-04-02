using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Data.Models;
using WebApplication1.Data.Specifications.Base;
using WebApplication1.ViewModels;

namespace WebApplication1.Data.Interfaces
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task UpdateUserAsync(User user, ProfileViewModel model);
        Task<User> GetUserAsync(string email);
        Task<User> GetUserAsync(int id);
        Task DeleteUserAsync(User user);
        Task<IReadOnlyList<User>> GetUsersAsync(ISpecification<User> specification);
        Task<IReadOnlyList<User>> GetUsersAsync();
        User CreateUser(RegisterViewModel model);
    }
}
