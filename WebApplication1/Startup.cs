using AspNetCore.SEOHelper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Globalization;
using System.IO.Compression;
using InternetShop.Data;
using InternetShop.Data.Interfaces;
using InternetShop.Data.Repository;
using ReflectionIT.Mvc.Paging;

namespace InternetShop
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
            AddLocalization(services);
            AddTransients(services);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddResponseCompression(options => options.EnableForHttps = true);
            services.Configure<BrotliCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddMemoryCache();
            services.AddSession();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new PathString("/Account/Login");
                    options.AccessDeniedPath = new PathString("/Account/Login");
                });
            services.AddPaging(options => {
                options.ViewName = "Bootstrap5";
                options.HtmlIndicatorDown = " <span>&darr;</span>";
                options.HtmlIndicatorUp = " <span>&uarr;</span>";
            });
        }

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
                if (context.Response.StatusCode == 535)
                {
                    context.Request.Path = "/Error/535";
                    await next();
                }
            });
            app.UseResponseCompression();
            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Add("Cache-Control", "public,max-age=604800");
                }
            });
            app.UseXMLSitemap(env.ContentRootPath);
            app.UseHttpsRedirection();
            //app.UseStatusCodePages();
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

        private static void AddLocalization(IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddControllersWithViews()
                .AddDataAnnotationsLocalization()
                .AddViewLocalization();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("de"),
                    new CultureInfo("ru")
                };

                options.DefaultRequestCulture = new RequestCulture("ru");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
        }
        private static void AddTransients(IServiceCollection services)
        {
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IOrdersRepository, OrdersRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IShopCart, ShopCartRepository>();
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<INewsRepository, NewsRepository>();
            services.AddTransient<ISiteRatingRepository, SiteRatingRepository>();
            services.AddTransient<IAttributeCategoryRepository, AttributeCategoryRepository>();
            services.AddTransient<IProductAttributeRepository, ProductAttributeRepository>();
        }
    }
}
