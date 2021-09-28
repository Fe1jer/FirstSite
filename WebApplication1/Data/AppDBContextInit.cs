using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using InternetShop.Data.Models;

namespace InternetShop.Data
{
    public class AppDBContextInit
    {
        public async static Task InitDbContextAsync(AppDBContext context)
        {
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();
            /*context.Order.RemoveRange(context.Order);
            context.OrderDetail.RemoveRange(context.OrderDetail);*/
            //context.SaveChanges();

            if (!await context.Roles.AnyAsync())
            {
                await context.Roles.AddRangeAsync(
                     new Role { Name = "user" },
                     new Role { Name = "moder" },
                    new Role { Name = "courier" },
                    new Role { Name = "admin" }
                    );
            }

            await context.SaveChangesAsync();
            if (!await context.Users.AnyAsync())
            {
                await context.Users.AddRangeAsync(
                     new User
                     {
                         Email = "vppechko@gmail.com",
                         Password = "AJbnFd7YQX6FAA2atuSnQqtJtROqXMc/KItA5WrYRkGTIds7+iAgDXd9xxpM7+NJyA==",
                         RoleId = 4,
                         Gender = "Male",
                         Name = "Viktor",
                         Img = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcScY-9qDVs2yQiXkeEHGQfvxEPLWHh-o53ZuQ&usqp=CAU"
                     },
                     new User
                     {
                         Email = "moder1@gmail.com",
                         Password = "AOVjJYFl9nJ6Zt/xxM1b7alZpqm46xyhsdkqxcGXWWnFSEKSj4s8F4B3CShd7AWEhA==",
                         RoleId = 2,
                         Gender = "Male",
                         Name = "Viktor",
                         Img = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcScY-9qDVs2yQiXkeEHGQfvxEPLWHh-o53ZuQ&usqp=CAU"
                     },
                    new User
                    {
                        Email = "courier1@gmail.com",
                        Password = "ALpvpQ90VUoOtGjKIWX8nFyfjFNnwoez+qN3QptLTECJMZDlx1DFtU+Rn/rBq1bFNw==",
                        RoleId = 3,
                        Gender = "Male",
                        Name = "Viktor",
                        Img = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcScY-9qDVs2yQiXkeEHGQfvxEPLWHh-o53ZuQ&usqp=CAU"
                    });
            }

            await context.SaveChangesAsync();
        }
    }
}