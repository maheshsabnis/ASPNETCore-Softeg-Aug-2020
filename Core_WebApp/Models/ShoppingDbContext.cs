
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Core_WebApp.Models
{
    public class ShoppingDbContext : DbContext
    {
       
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }


        // read the Connection string from the appSettings.json
        // Register the  ShoppingDbContext class in DI Container of the Application
        // in Startup class an its ConfigureService method
        // to read the connection information and start performing mapping
        // CRUD operations, create a constructor for the class and inject the
        // DbContextOptions<T> class in it. Where T is ShoppingDbContext class

        public ShoppingDbContext(DbContextOptions<ShoppingDbContext> options): base(options)
        {
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // define the one-to-many and many-to-one relationship
            // across the Category-to-Product tables

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category) // hasone category for each product
                .WithMany(c => c.Products) // one categoey contains multiple products
                .HasForeignKey(p => p.CategoryRowId); // Foreign Key Relationship 

        }
    }
}
