using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using Core_WebApp.CustomSessions;
using Core_WebApp.Models;
using Core_WebApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Core_WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IRepository<Product,int> prdRepo;
        private readonly IRepository<Category, int> catRepo;

        public ProductController(IRepository<Product, int> prdRepo, IRepository<Category, int> catRepo)
        {
            this.prdRepo = prdRepo;
            this.catRepo = catRepo;
        }

        /// <summary>
        /// Http Get methdo to return Index view with List of prdegories
        /// </summary>
        /// <returns></returns>
        public  IActionResult Index()
        {
            // read data of category from the session
            Category cat = HttpContext.Session.GetSessionData<Category>("cat");

            var catRowId = cat.CategoryRowId;

            List<Product> prds = new List<Product>();
            if (catRowId > 0)
            {
                prds = prdRepo.GetAsync()
                    .Result.ToList()
                    .Where(c=>c.CategoryRowId == catRowId).ToList();
            }
            else
            {
                 prds =  prdRepo.GetAsync().Result.ToList();
            }

          
            return View(prds);
        }

        /// <summary>
        /// Http Get method that will return empty View for accepting prdeogry Data 
        /// </summary>
        /// <returns></returns>
        public  IActionResult Create()
        {
            var prd = new Product();
            
            return View(prd);
        }

        /// <summary>
        /// Http Post method to accept Product data from View request 
        /// </summary>
        /// <param name="prd"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(Product prd)
        {
            // validate the model
            if (ModelState.IsValid)
            {
                prd = await prdRepo.CreateAsync(prd);
                // return the Index action methods from
                // the current controller
                return RedirectToAction("Index");
            }
            return View(prd); // stey on same page and show error messages
        }
    }
}
