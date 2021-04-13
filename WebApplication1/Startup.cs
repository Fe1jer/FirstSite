using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApplication1.Data;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Repository;

namespace WebApplication1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDBContext>(options => options.UseSqlServer(connection));

            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IOrdersRepository, OrdersRepository>();
            services.AddTransient<IProductsManager, ProductsManagerRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IShopCart, ShopCartRepository>();
            services.AddTransient<IPasswordHasher, PasswordHasherRepository>();
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IProductsManager, ProductsManagerRepository>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddMemoryCache();
            services.AddSession();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new PathString("/Account/Login");
                    options.AccessDeniedPath = new PathString("/Account/Login");
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404)
                {
                    context.Request.Path = "/Error/404";
                    await next();
                }
            });
            //app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseMvcWithDefaultRoute();
            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "admin", template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}
