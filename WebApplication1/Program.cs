using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using InternetShop.Data;
using Microsoft.AspNetCore.Identity;
using InternetShop.Data.Models;

namespace InternetShop
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            await InitContext(host);
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private async static Task InitContext(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDBContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var rolesManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            await AppDBContextInit.InitDbContextAsync(userManager, rolesManager, context);
        }
    }
}
