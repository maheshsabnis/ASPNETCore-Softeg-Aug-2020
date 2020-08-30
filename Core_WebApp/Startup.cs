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
            services.AddDbContext<ShoppingDbContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("eShoppingDbConnection"));
            });

            // regiter Category and Product Repositories
            services.AddScoped<IRepository<Category, int>, CategoryRepository>();
            services.AddScoped<IRepository<Product, int>, ProductRepository>();

            // define sessions
            // session will be stored in Cache
            services.AddDistributedMemoryCache();
            services.AddSession(session=> {
                session.IdleTimeout = TimeSpan.FromMinutes(20);
            });


            services.AddControllersWithViews();
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
