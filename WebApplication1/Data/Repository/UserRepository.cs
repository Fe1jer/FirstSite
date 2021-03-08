using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Models;
using WebApplication1.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace WebApplication1.Data.Repository
{
    public class UserRepository : IAllUser
    {
        private readonly AppDBContext appDBContent;

        public UserRepository(AppDBContext appDBContent)
        {
            this.appDBContent = appDBContent;
        }

        public List<User> Users => appDBContent.Users.Include(p=>p.Role).OrderByDescending(p=>p.RoleId).ToList();

        public List<Role> Roles => appDBContent.Roles.ToList();

        public async Task<User> User(LoginViewModel model) => await appDBContent.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);

        public async Task<User> User(int id) => await appDBContent.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id);

        public async Task<User> EmailUser(RegisterViewModel model) => await appDBContent.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

        public async Task<Role> GetRole(string role) => await appDBContent.Roles.FirstOrDefaultAsync(u => u.Name == role);

        public User GetUserEmail(string email) =>  appDBContent.Users.Include(u => u.Role).FirstOrDefault(u => u.Email == email);

        public List<User> SearchUsers(string email) =>  appDBContent.Users.Include(u => u.Role).Where(p=>p.Email.Contains(email)).ToList();

        public void SetUserRole(User user, string role)
        {
            user = appDBContent.Users.FirstOrDefault(p=>p==user);
            user.Role = appDBContent.Roles.FirstOrDefault(u => u.Name == role);
            appDBContent.SaveChanges();
        }

        public void AddUser(User user)
        {
            appDBContent.Users.Add(user);
             appDBContent.SaveChanges();
        }
    }
}
