using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data.Models;
using WebApplication1.Data.Specifications.Base;
using WebApplication1.ViewModels;

namespace WebApplication1.Data.Interfaces
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);
        Task UpdateUserRole(User user);
        Task<User> GetUserAsync(string email); 
        Task<User> GetUserAsync(int id); 
        Task<IReadOnlyList<User>> GetUsersAsync(ISpecification<User> specification);
        Task<IReadOnlyList<User>> GetUsersAsync();
    }
}
