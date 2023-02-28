using CmsShoppingCart.Infrastructure;
using CmsShoppingCart.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShoppingCart.Controllers
{
    public class PagesController : Controller
    {
        private readonly CmsShoppingCartContext context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public PagesController(CmsShoppingCartContext context, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        //GET / or /slug
        public async Task<IActionResult> Page(string slug)
        {
            if (slug == null)
            {
                return View(await context.Pages.Where(p => p.Slug == "home").FirstOrDefaultAsync());
            }
            Page page = await context.Pages.Where(p => p.Slug == slug).FirstOrDefaultAsync();

            if (page == null)
            {
                return NotFound();
            }
            return View(page);
        }
    }
}
