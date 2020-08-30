using Core_WebApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core_WebApp.Repositories
{
    public class ProductRepository : IRepository<Product, int>
    {
         private readonly ShoppingDbContext context;
        /// <summary>
        ///  Inject the ShoppingDbContext
        /// </summary>
        public ProductRepository(ShoppingDbContext context)
        {
            this.context = context;
        }
        public async Task<Product> CreateAsync(Product entity)
        {
            var res = await context.Products.AddAsync(entity);
            await context.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var prd = await context.Products.FindAsync(id);
            if (prd == null) return false;

            context.Products.Remove(prd);
            return true;
        }

        public async Task<IEnumerable<Product>> GetAsync()
        {
            return await context.Products.ToListAsync();
        }

        public async Task<Product> GetAsync(int id)
        {
            return await context.Products.FindAsync(id);
        }

        public async Task<Product> UpdateAsync(int id, Product entity)
        {
            var prd = await context.Products.FindAsync(id);
            if (prd != null)
            {
                prd.ProductId = entity.ProductId;
                prd.ProductName = entity.ProductName;
                prd.Manufacturer = entity.Manufacturer;
                prd.CategoryRowId = entity.CategoryRowId;
                prd.Price = entity.Price;
                await context.SaveChangesAsync();
                return prd;
            }
            return entity;

        }
    }
}
