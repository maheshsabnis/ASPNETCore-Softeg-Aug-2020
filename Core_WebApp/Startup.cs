using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Core_WebApp.Models;
using Microsoft.EntityFrameworkCore;
using Core_WebApp.Repositories;
using Core_WebApp.CustomFilters;
using Core_WebApp.Data;
using Microsoft.AspNetCore.Identity;

namespace Core_WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // regisater the ShoppingDbContext class in DI COntainer
            services.AddDbContext<ShoppingDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("eShoppingDbConnection"));
            });

                services.AddDbContext<SecurityAuthDbContext>(options =>
                    options.UseSqlServer(
                        Configuration.GetConnectionString("SecurityAuthDbContextConnection")));

                services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<SecurityAuthDbContext>();

            services.AddAuthentication();
           
            // regiter Category and Product Repositories
            services.AddScoped<IRepository<Category, int>, CategoryRepository>();
            services.AddScoped<IRepository<Product, int>, ProductRepository>();

            // define sessions
            // session will be stored in Cache
            services.AddDistributedMemoryCache();
            services.AddSession(session=> {
                session.IdleTimeout = TimeSpan.FromMinutes(20);
            });

            // method is used to handle requests for MVC and API Controllers
            // use the Overload of AddControllersWithViews() method that
            // uses MvcOptions to register/apply filter in global scope
            services.AddControllersWithViews(options => {
               // options.Filters.Add(new LogFilter());
                options.Filters.Add(typeof(AppExceptionFilter)); // resolve the modelmetadataprovider
            });
            // Request Processing for Razor Web Forms for identity
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
               

            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // use sessions
            app.UseSession();

            // routing
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages(); // used for the Razor Web Forms for Idnetity  Pages
            });
        }
    }
}
