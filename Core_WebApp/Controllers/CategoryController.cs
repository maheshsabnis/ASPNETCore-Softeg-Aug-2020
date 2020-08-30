using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core_WebApp.CustomSessions;
using Core_WebApp.Models;
using Core_WebApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Core_WebApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IRepository<Category, int> catRepo;
        /// <summary>
        /// Inject the Category Repository
        /// </summary>
        public CategoryController(IRepository<Category, int> catRepo)
        {
            this.catRepo = catRepo;
        }
        /// <summary>
        /// Http Get methdo to return Index view with List of Categories
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var cats = await catRepo.GetAsync();
            return View(cats);
        }

        /// <summary>
        /// Http Get method that will return empty View for accepting Cateogry Data 
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            var cat = new Category();
            return View(cat);
        }

        /// <summary>
        /// Http Post method to accept Category data from View request 
        /// </summary>
        /// <param name="cat"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(Category cat)
        {
            try
            {
                // validate the model
                if (ModelState.IsValid)
                {
                    if (cat.BasePrice < 0)
                        throw new Exception("Base Price cannot be -ve");
                    cat = await catRepo.CreateAsync(cat);
                    // return the Index action methods from
                    // the current controller
                    return RedirectToAction("Index");
                }
                return View(cat); // stey on same page and show error messages
            }
            catch (Exception ex)
            {
                // return to error page
                return View("Error", new ErrorViewModel() 
                { 
                  ControllerName = this.RouteData.Values["controller"].ToString(),
                  ActionName = this.RouteData.Values["action"].ToString(),
                  ErrorMessage = ex.Message
                });
            }
        }

        public IActionResult ShowDetails(int id)
        {
            Category cat = catRepo.GetAsync(id).Result;
            HttpContext.Session.SetSessionData<Category>("cat", cat);

            return RedirectToAction("Index", "Product");
        }
    }
}
