using CmsShoppingCart.Infrastructure;
using CmsShoppingCart.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShoppingCart.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin, editor")]
    [Area("Admin")]
    public class PagesController : Controller
    {
        #region Dependency Injection
        private readonly CmsShoppingCartContext context;

        public PagesController(CmsShoppingCartContext context)
        {
            this.context = context;
        }
        #endregion

        // GET /admin/pages
        public async Task<IActionResult> Index() // return ordered list of pages 
        {
            IQueryable<Page> pages = from p in context.Pages
                                     orderby p.Sorting
                                     select p;
            List<Page> pagesList = await pages.ToListAsync();
            return View(pagesList);
        }

        // GET /admin/pages/details/5
        public async Task<IActionResult> Details(int id)
        {
            #region Check if page exist
            var page = await context.Pages.FirstOrDefaultAsync(p => p.Id == id);
            if (page == null) return NotFound();
            #endregion

            return View(page);
        }

        // GET /admin/pages/create
        public IActionResult Create() => View();

        // POST /admin/pages/create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Page page)
        {
            if (ModelState.IsValid)
            {
                #region initialize Slug and Sorting attribute
                page.Slug = page.Title.ToLower().Replace(" ", "-");
                page.Sorting = 100;
                #endregion

                #region Check if there is a page with the same slug
                var slug = await context.Pages.FirstOrDefaultAsync(x => x.Slug == page.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The page is already exist");
                    return View(page);
                }
                #endregion

                context.Add(page);
                await context.SaveChangesAsync();
                TempData["Success"] = "The page has been added!";
                return RedirectToAction("Index");
            }
            return View(page);
        }

        // GET /admin/pages/edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var page = await context.Pages.FindAsync(id);
            if (page == null) return NotFound();

            return View(page);
        }

        // POST /admin/pages/edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Page page)
        {
            if (ModelState.IsValid)
            {
                // set the slug of page with id = 1 to home whatever the name of it
                page.Slug = page.Id == 1 ? "home" : page.Title.ToLower().Replace(" ", "-");

                #region Check if there is a page with the same slug name but not the edited page
                // get all pages but not the edit page and the check if there is another page with the same slug name of edit page 
                var slug = await context.Pages.Where(p => p.Id != page.Id).FirstOrDefaultAsync(x => x.Slug == page.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The page is already exist");
                    return View(page);
                }
                #endregion

                context.Update(page);
                await context.SaveChangesAsync();
                TempData["Success"] = "The page has been edited successfully!";
                return RedirectToAction("Edit", new { page.Id });
            }
            return View(page);
        }

        // GET /admin/pages/edit/5
        public async Task<IActionResult> Delete(int id)
        {
            var page = await context.Pages.FindAsync(id);
            if (page == null)
            {
                TempData["Error"] = "The page doesn't exist!";
            }
            else
            {
                context.Pages.Remove(page);
                await context.SaveChangesAsync();
                TempData["Success"] = "The page has been deleted successfully!";
            }

            return RedirectToAction("Index");
        }

        // POST /admin/pages/reorder
        [HttpPost]
        public async Task<IActionResult> Reorder(int[] id) // id post from js ui in index view, the name is id not ids, check order post in network inspect
        {
            int count = 1; //first sorting number is one because home page is zero

            foreach (var pageId in id)
            {
                Page page = await context.Pages.FindAsync(pageId);
                page.Sorting = count;
                context.Update(page);
                await context.SaveChangesAsync();
                count++;
            }
            return Ok();

        }

    }
}
