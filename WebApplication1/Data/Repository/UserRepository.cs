using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Models;
using WebApplication1.Data.Specifications;
using WebApplication1.Data.Specifications.Base;
using WebApplication1.ViewModels;

namespace WebApplication1.Data.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly IPasswordHasher _passwordHasher;

        public UserRepository(AppDBContext appDBContext, IPasswordHasher passwordHasher) : base(appDBContext)
        {
            _passwordHasher = passwordHasher;
        }

        public async Task<User> GetUserAsync(string email)
        {
            var users = await GetAllAsync(new UserSpecification().IncludeRole());
            return users.FirstOrDefault(u => u.Email == email);
        }

        public async Task<User> GetUserAsync(int id)
        {
            var users = await GetAllAsync(new UserSpecification().IncludeRole());
            return users.FirstOrDefault(u => u.Id == id);
        }

        public async Task<IReadOnlyList<User>> GetUsersAsync()
        {
            return await GetAllAsync();
        }

        public async Task<IReadOnlyList<User>> GetUsersAsync(ISpecification<User> specification)
        {
            return await GetAllAsync(specification);
        }

        public async Task UpdateUserAsync(User user)
        {
            await UpdateAsync(user);
        }

        public async Task DeleteUserAsync(User user)
        {
            await DeleteAsync(user);
        }

        public async Task UpdateUserAsync(User user, ProfileViewModel model)
        {
            user.Email = model.Email;
            user.Name = model.Name;
            user.Surname = model.Surname;
            user.Patronymic = model.Patronymic;
            user.Gender = model.Gender;
            user.PhoneNumber = model.PhoneNumber;
            user.DateOfBirth = model.DateOfBirth;
            await UpdateAsync(user);
        }

        public async Task AddUserAsync(User user)
        {
            await AddAsync(user);
        }

        public User CreateUser(RegisterViewModel model)
        {
            User user = new User
            {
                Email = model.Email,
                Password = _passwordHasher.HashPassword(model.Password),
                Gender = model.Gender,
                Name = model.Name,
                LockoutEnd = DateTime.Now.AddMinutes(5),
                Img = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcScY-9qDVs2yQiXkeEHGQfvxEPLWHh-o53ZuQ&usqp=CAU"
            };

            return user;
        }
    }
}
