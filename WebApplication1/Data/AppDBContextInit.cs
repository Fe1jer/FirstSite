using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using InternetShop.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Runtime.InteropServices;

namespace InternetShop.Data
{
    public class AppDBContextInit
    {
        public async static Task InitDbContextAsync(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager, AppDBContext context)
        {
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();
            /*context.Order.RemoveRange(context.Order);
            context.OrderDetail.RemoveRange(context.OrderDetail);*/
            //context.SaveChanges();
            if (!await roleManager.Roles.AnyAsync())
            {
                await roleManager.CreateAsync(new IdentityRole<int>("user"));
                await roleManager.CreateAsync(new IdentityRole<int>("moderator"));
                await roleManager.CreateAsync(new IdentityRole<int>("courier"));
                await roleManager.CreateAsync(new IdentityRole<int>("admin"));
            }
            if (!await userManager.Users.AnyAsync())
            {
                User admin = new User
                {
                    Email = "vppechko@gmail.com",
                    Gender = "Male",
                    UserName = "vppechko@gmail.com",
                    Name = "Viktor",
                    Img = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcScY-9qDVs2yQiXkeEHGQfvxEPLWHh-o53ZuQ&usqp=CAU"
                };
                await userManager.CreateAsync(admin, "Fe1jer_Degra1der");
                await userManager.AddToRoleAsync(admin, "admin");

                User courier = new User
                {
                    Email = "courier1@gmail.com",
                    Gender = "Male",
                    UserName = "courier1@gmail.com",
                    Name = "Viktor",
                    Img = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcScY-9qDVs2yQiXkeEHGQfvxEPLWHh-o53ZuQ&usqp=CAU"
                };
                await userManager.CreateAsync(courier, "courier1");
                await userManager.AddToRoleAsync(courier, "courier");

                User moder = new User
                {
                    Email = "moder1@gmail.com",
                    Gender = "Male",
                    UserName = "moder1@gmail.com",
                    Name = "Viktor",
                    Img = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcScY-9qDVs2yQiXkeEHGQfvxEPLWHh-o53ZuQ&usqp=CAU"
                };
                await userManager.CreateAsync(moder, "moder1");
                await userManager.AddToRoleAsync(moder, "moder");
            }
        }
    }
}