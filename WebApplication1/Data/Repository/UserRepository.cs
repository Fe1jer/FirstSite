using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Models;
using WebApplication1.Data.Services;
using WebApplication1.Data.Specifications;
using WebApplication1.Data.Specifications.Base;
using WebApplication1.ViewModels;

namespace WebApplication1.Data.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {

        public UserRepository(AppDBContext appDBContext) : base(appDBContext)
        {
        }

        public async Task<User> GetUserAsync(string email)
        {
            var users = await base.GetAllAsync(new UserSpecification().IncludeRole());
            return users.FirstOrDefault(u => u.Email == email);
        }

        public async Task<User> GetUserAsync(int id)
        {
            var users = await base.GetAllAsync(new UserSpecification().IncludeRole());
            return users.FirstOrDefault(u => u.Id == id);
        }

        public new async Task<IReadOnlyList<User>> GetAllAsync()
        {
            return await base.GetAllAsync();
        }

        public new async Task<IReadOnlyList<User>> GetAllAsync(ISpecification<User> specification)
        {
            return await base.GetAllAsync(specification);
        }

        public new async Task UpdateAsync(User user)
        {
            await base.UpdateAsync(user);
        }

        public new async Task DeleteAsync(User user)
        {
            await base.DeleteAsync(user);
        }

        public async Task UpdateAsync(User user, ProfileViewModel model)
        {
            user.Email = model.Email;
            user.Name = model.Name;
            user.Surname = model.Surname;
            user.Patronymic = model.Patronymic;
            user.Gender = model.Gender;
            user.PhoneNumber = model.PhoneNumber;
            user.DateOfBirth = model.DateOfBirth;
            await base.UpdateAsync(user);
        }

        public new async Task AddAsync(User user)
        {
            await base.AddAsync(user);
        }

        public User CreateUser(RegisterViewModel model)
        {
            User user = new User
            {
                Email = model.Email,
                Password = HashingService.HashPassword(model.Password),
                Gender = model.Gender,
                Name = model.Name,
                LockoutEnd = DateTime.Now.AddMinutes(5),
                Img = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcScY-9qDVs2yQiXkeEHGQfvxEPLWHh-o53ZuQ&usqp=CAU"
            };

            return user;
        }
    }
}
