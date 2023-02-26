using CmsShoppingCart.Infrastructure;
using CmsShoppingCart.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShoppingCart.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly CmsShoppingCartContext context;

        public CategoriesController(CmsShoppingCartContext context)
        {
            this.context = context;
        }

        // GET /admin/categories
        public async Task<IActionResult> Index()
        {
            return View(await context.Categories.OrderBy(x => x.Sorting).ToListAsync());
        }

        // GET /admin/categories/create
        public IActionResult Create() => View();

        // POST /admin/pages/create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                category.Slug = category.Name.ToLower().Replace(" ", "-");
                category.Sorting = 100;
                var slug = await context.Pages.FirstOrDefaultAsync(x => x.Slug == category.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The category is already exist");
                    return View(category);
                }
                context.Add(category);
                await context.SaveChangesAsync();
                TempData["Success"] = "The category has been added!";
                return RedirectToAction("Index");
            }
            return View(category);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var category = await context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            return View(category);
        }
        // POST /admin/categories/edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (ModelState.IsValid)
            {
                category.Slug = category.Id == 1 ? "home" : category.Name.ToLower().Replace(" ", "-");
                // get all pages but not the edit category and the check if there is another category with the same slug name of edit category 
                var slug = await context.Pages.Where(p => p.Id != category.Id).FirstOrDefaultAsync(x => x.Slug == category.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The category is already exist");
                    return View(category);
                }
                context.Update(category);
                await context.SaveChangesAsync();
                TempData["Success"] = "The category has been edited successfully!";
                return RedirectToAction("Edit", new { id });
            }
            return View(category);
        }

        // GET /admin/pages/edit/5
        public async Task<IActionResult> Delete(int id)
        {
            var category = await context.Categories.FindAsync(id);
            if (category == null)
            {
                TempData["Error"] = "The category doesn't exist!";
            }
            else
            {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();
                TempData["Success"] = "The category has been deleted successfully!";
            }

            return RedirectToAction("Index");
        }

    }
}
