using System.Collections.Generic;
using System.Threading.Tasks;
using InternetShop.Data.Models;
using InternetShop.Data.Specifications.Base;
using InternetShop.ViewModels;

namespace InternetShop.Data.Interfaces
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
