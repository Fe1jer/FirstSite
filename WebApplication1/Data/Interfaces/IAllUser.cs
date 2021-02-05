using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Data.Interfaces
{
    public interface IAllUser
    {
        List<User> Users { get; }
        List<Role> Roles { get; }
        public void AddUser(User user);
        public void SetUserRole(User user, string role);
        public Task<User> User(LoginViewModel model); 
        public Task<User> User(int id); 
        public Task<User> EmailUser(RegisterViewModel model);
        public Task<Role> GetRole(string role);
        public User GetUserEmail(string email);
    }
}
