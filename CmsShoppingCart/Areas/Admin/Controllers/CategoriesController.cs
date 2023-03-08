using CmsShoppingCart.Infrastructure;
using CmsShoppingCart.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShoppingCart.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        #region Dependency Injection  => to inject the context service
        private readonly CmsShoppingCartContext context;
        public CategoriesController(CmsShoppingCartContext context)
        {
            this.context = context;
        }
        #endregion

        #region Index => return ordered list of categories
        // GET /admin/categories
        public async Task<IActionResult> Index()
        {
            return View(await context.Categories.OrderBy(x => x.Sorting).ToListAsync());
        }
        #endregion

        #region Create  => two action, get and post to create a new category

        // GET /admin/categories/create
        public IActionResult Create() => View(); //

        // POST /admin/pages/create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                #region Initialize Slug and Sorting attributes
                category.Slug = category.Name.ToLower().Replace(" ", "-"); //set slug name here to be lower with dash '-'
                category.Sorting = 100; // 
                #endregion

                #region Check if there is a page with the same slug
                var slug = await context.Categories.FirstOrDefaultAsync(x => x.Slug == category.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The category is already exist");
                    return View(category);
                }
                #endregion

                #region Add a category
                context.Add(category);
                await context.SaveChangesAsync();
                #endregion

                #region Success message passed to a view
                TempData["Success"] = "The category has been added!";
                #endregion

                return RedirectToAction("Index");
            }
            return View(category);
        }
        #endregion

        #region Edit  => two action to edit a category
        // GET /admin/categories/edit/5
        public async Task<IActionResult> Edit(int id)
        {
            #region Check if the category exist
            var category = await context.Categories.FindAsync(id);
            if (category == null) return NotFound();
            #endregion

            return View(category);
        }

        // POST /admin/categories/edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (ModelState.IsValid)
            {
                category.Slug = category.Name.ToLower().Replace(" ", "-");

                #region Check if there is a category with the same slug
                // get all pages but not the edit category and the check if there is another category with the same slug name of edit category 
                var slug = await context.Categories.Where(p => p.Id != category.Id).FirstOrDefaultAsync(x => x.Slug == category.Slug);

                if (slug != null)
                {
                    ModelState.AddModelError("", "The category is already exist");
                    return View(category);
                }
                #endregion

                #region Update the category
                context.Update(category);
                await context.SaveChangesAsync();
                #endregion

                #region Success message passed to a view
                TempData["Success"] = "The category has been edited successfully!";
                #endregion

                return View(category);

                //return RedirectToAction("Edit", new { id });//Redirect to edit get action
            }
            return View(category);
        }
        #endregion

        #region Delete => to delete a category

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
        #endregion

    }
}
