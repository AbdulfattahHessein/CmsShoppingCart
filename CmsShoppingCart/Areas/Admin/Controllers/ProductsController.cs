using CmsShoppingCart.Infrastructure;
using CmsShoppingCart.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShoppingCart.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("Admin")]
    public class ProductsController : Controller
    {
        #region Dependency Injection
        private readonly CmsShoppingCartContext context;

        private readonly IWebHostEnvironment webHostEnvironment;

        public ProductsController(CmsShoppingCartContext context, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            this.webHostEnvironment = webHostEnvironment;
        }
        #endregion

        // GET /admin/products
        public async Task<IActionResult> Index(int p = 1) //you must create a new route take "p" instead "id"
        {
            #region Pagination
            int pageSize = 6;
            var products = context.Products.OrderByDescending(x => x.Id)
                                            .Include(x => x.Category)
                                            .Skip((p - 1) * pageSize) // if p = 2, this mean the first sex elements are skipped
                                            .Take(pageSize); // return the first 6 elements from the remaining list returned from skip()  
            #endregion

            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)context.Products.Count() / pageSize);

            return View(await products.ToListAsync());
        }

        // GET /admin/products/details/5
        public async Task<IActionResult> Details(int id)
        {
            var product = await context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();

            return View(product);
        }

        // GET /admin/products/create
        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.Sorting), nameof(Category.Id), nameof(Category.Name));

            return View();
        }

        // POST /admin/products/create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.Sorting), nameof(Category.Id), nameof(Category.Name));

            if (ModelState.IsValid)
            {
                // initialize slug value
                product.Slug = product.Name.ToLower().Replace(" ", "-");

                #region Check if there is a product with the same slug
                var slug = await context.Products.FirstOrDefaultAsync(x => x.Slug == product.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The product is already exist");
                    return View(product);
                }
                #endregion

                #region Upload an image or set a default image
                string imageName = "noimage.png";
                if (product.ImageUpload != null)
                {
                    string uploadDir = Path.Combine(webHostEnvironment.WebRootPath, "media/products");

                    imageName = Guid.NewGuid().ToString() + " " + product.ImageUpload.FileName;

                    string filePath = Path.Combine(uploadDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);

                    await product.ImageUpload.CopyToAsync(fs);

                    fs.Close();
                }

                product.Image = imageName;
                #endregion

                context.Add(product);
                await context.SaveChangesAsync();
                TempData["Success"] = "The product has been added!";
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET /admin/pages/edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var product = await context.Products.FindAsync(id);
            if (product == null) return NotFound();

            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.Sorting), nameof(Category.Id), nameof(Category.Name));

            return View(product);
        }

        // POST /admin/pages/edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.Sorting), nameof(Category.Id), nameof(Category.Name));

            if (ModelState.IsValid)
            {
                product.Slug = product.Name.ToLower().Replace(" ", "-");
                // get all pages but not the edit product and the check if there is another product with the same slug name of edit product 
                var slug = await context.Products.Where(p => p.Id != product.Id).FirstOrDefaultAsync(x => x.Slug == product.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The product is already exist");
                    return View(product);
                }
                if (product.ImageUpload != null)
                {
                    string uploadDir = Path.Combine(webHostEnvironment.WebRootPath, "media/products");
                    if (!string.Equals(product.Image, "noimage.png"))
                    {
                        string oldImagePath = Path.Combine(uploadDir, product.Image);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    var imageName = Guid.NewGuid().ToString() + " " + product.ImageUpload.FileName;

                    string filePath = Path.Combine(uploadDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);

                    await product.ImageUpload.CopyToAsync(fs);

                    fs.Close();

                    product.Image = imageName;
                }
                context.Update(product);
                await context.SaveChangesAsync();
                TempData["Success"] = "The product has been edited successfully!";
                return RedirectToAction("Edit", new { product.Id });
            }
            return View(product);
        }

        // GET /admin/pages/edit/5
        public async Task<IActionResult> Delete(int id)
        {
            var product = await context.Products.FindAsync(id);
            var productPageIndex = Array.IndexOf(context.Products.ToArray(), product);

            if (product == null)
            {
                TempData["Error"] = "The product doesn't exist!";
            }
            else
            {
                string uploadDir = Path.Combine(webHostEnvironment.WebRootPath, "media/products");
                if (!string.Equals(product.Image, "noimage.png"))
                {
                    string oldImagePath = Path.Combine(uploadDir, product.Image);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                context.Products.Remove(product);
                await context.SaveChangesAsync();
                TempData["Success"] = "The product has been deleted successfully!";
            }

            //return RedirectToAction("Index" /*, new { p = Math.Ceiling((decimal)productPageIndex) % 6 }*/);
            return Redirect(Request.Headers["Referer"].ToString());
        }



    }
}
