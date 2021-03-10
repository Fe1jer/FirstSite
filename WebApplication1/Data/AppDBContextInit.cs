using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApplication1.Data.Models;

namespace WebApplication1.Data
{
    public class AppDBContextInit
    {
        public async static Task InitDbContextAsync(AppDBContext context)
        {
            /*            context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();*/
/*            context.Order.RemoveRange(context.Order);
            context.OrderDetail.RemoveRange(context.OrderDetail);*/
            //context.SaveChanges();

            if (!await context.Roles.AnyAsync())
                await context.Roles.AddRangeAsync(
                     new Role { Name = "admin" }
                     ,
                     new Role { Name = "courier" }
                     ,
                     new Role { Name = "moderator" }
                     ,
                    new Role { Name = "user" }
                    );

            if (!await context.Users.AnyAsync())
                await context.Users.AddRangeAsync(
                     new User
                     {
                         Email = "vppechko@gmail.com",
                         Password = "AJbnFd7YQX6FAA2atuSnQqtJtROqXMc/KItA5WrYRkGTIds7+iAgDXd9xxpM7+NJyA==",
                         RoleId = 4
                     },
                     new User
                     {
                         Email = "moder1@gmail.com",
                         Password = "AOVjJYFl9nJ6Zt/xxM1b7alZpqm46xyhsdkqxcGXWWnFSEKSj4s8F4B3CShd7AWEhA==",
                         RoleId = 2
                     },
                    new User
                    {
                        Email = "courier1@gmail.com",
                        Password = "ALpvpQ90VUoOtGjKIWX8nFyfjFNnwoez+qN3QptLTECJMZDlx1DFtU+Rn/rBq1bFNw==",
                        RoleId = 3
                    });
            await context.SaveChangesAsync();
        }

    }
}