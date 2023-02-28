using CmsShoppingCart.Infrastructure;
using CmsShoppingCart.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShoppingCart.Controllers
{
    public class ProductsController : Controller
    {
        private readonly CmsShoppingCartContext context;

        private readonly IWebHostEnvironment webHostEnvironment;

        public ProductsController(CmsShoppingCartContext context, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            this.webHostEnvironment = webHostEnvironment;
        }
        // GET /admin/products
        public async Task<IActionResult> Index(int p = 1) //you must create a new route take "p" instead "id"
        {
            int pageSize = 6;
            var products = context.Products.OrderByDescending(x => x.Id)
                                            .Skip((p - 1) * pageSize) // if p = 2, this mean the first sex elements are skipped
                                            .Take(pageSize); // return the first 6 elements from the remaining list returned from skip() 

            return View(await products.ToListAsync());
        }

        // GET /products/category
        public async Task<IActionResult> ProductsByCategory(string categorySlug, int p = 1) //you must create a new route take "p" instead "id"
        {
            Category category = await context.Categories.Where(c => c.Slug == categorySlug).FirstOrDefaultAsync();
            if (category == null) return RedirectToAction("Index");
            int pageSize = 3;
            var products = context.Products.OrderByDescending(x => x.Id)
                                            .Where(p => p.CategoryId == category.Id)
                                            .Skip((p - 1) * pageSize) // if p = 2, this mean the first sex elements are skipped
                                            .Take(pageSize); // return the first 6 elements from the remaining list returned from skip() 
            ViewBag.CategoryName = category.Name;

            return View(await products.ToListAsync());
        }

    }
}
