using System.Collections.Generic;
using System.Linq;
using WebApplication1.Data.Models;

namespace WebApplication1.Data
{
    public class DBObjects
    {
        public static void Initial(AppDBContext context)
        {
            /*            context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();*/
/*            context.Order.RemoveRange(context.Order);
            context.OrderDetail.RemoveRange(context.OrderDetail);*/
            //context.SaveChanges();

            if (!context.Roles.Any())
                context.Roles.AddRange(
                     new Role { Name = "admin" }
                     ,
                     new Role { Name = "courier" }
                     ,
                     new Role { Name = "moderator" }
                     ,
                    new Role { Name = "user" }
                    );

            if (!context.Users.Any())
                context.Users.AddRange(
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
            context.SaveChanges();
        }

    }
}