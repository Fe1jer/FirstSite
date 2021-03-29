using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Models;
using WebApplication1.Data.Specifications;
using WebApplication1.Data.Specifications.Base;

namespace WebApplication1.Data.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDBContext appDBContext) : base(appDBContext)
        {

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

        public async Task UpdateUser(User user)
        {
            await UpdateAsync(user);
        }

        public async Task AddUserAsync(User user)
        {
            await AddAsync(user);
        }
    }
}
