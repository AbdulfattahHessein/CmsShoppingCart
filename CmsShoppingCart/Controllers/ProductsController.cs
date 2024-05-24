using CmsShoppingCart.Infrastructure;
using CmsShoppingCart.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShoppingCart.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly CmsShoppingCartContext context;

        public ProductsController(CmsShoppingCartContext context)
        {
            this.context = context;
        }
        // GET /products
        public async Task<IActionResult> Index(int p = 1) //you must create a new route take "p" instead "id"
        {
            int pageSize = 6;
            var products = context.Products.OrderByDescending(x => x.Id)
                                            .Skip(( p - 1 ) * pageSize) // if p = 2, this mean the first sex elements are skipped
                                            .Take(pageSize); // return the first 6 elements from the remaining list returned from skip() 

            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)context.Products.Count() / pageSize);

            return View(await products.ToListAsync());
        }

        // GET /products/category
        public async Task<IActionResult> ProductsByCategory(string categorySlug, int p = 1) //you must create a new route take "p" instead "id"
        {
            Category category = await context.Categories.Where(c => c.Slug == categorySlug).FirstOrDefaultAsync();
            if (category == null)
                return RedirectToAction("Index");
            int pageSize = 6;
            var products = context.Products.OrderByDescending(x => x.Id)
                                            .Where(p => p.CategoryId == category.Id)
                                            .Skip(( p - 1 ) * pageSize) // if p = 2, this mean the first sex elements are skipped
                                            .Take(pageSize); // return the first 6 elements from the remaining list returned from skip() 
            ViewBag.CategoryName = category.Name;
            ViewBag.CategorySlug = category.Slug;

            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;

            var productsByCategory = context.Products.OrderByDescending(x => x.Id).Where(p => p.CategoryId == category.Id);

            ViewBag.TotalPages = (int)Math.Ceiling((decimal)productsByCategory.Count() / pageSize);

            return View(await products.ToListAsync());
        }

    }
}
