using CmsShoppingCart.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShoppingCart.Infrastructure.ViewComponents
{
    public class CategoriesViewComponent : ViewComponent
    {
        private readonly CmsShoppingCartContext context;

        public CategoriesViewComponent(CmsShoppingCartContext context)
        {
            this.context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var Categories = await GetCategoriesAsync();
            return View(Categories);
        }

        private Task<List<Category>> GetCategoriesAsync()
        {
            return context.Categories.OrderBy(p => p.Sorting).ToListAsync();
        }
    }
}
