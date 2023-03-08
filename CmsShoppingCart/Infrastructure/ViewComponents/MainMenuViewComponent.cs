using CmsShoppingCart.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShoppingCart.Infrastructure.ViewComponents
{
    public class MainMenuViewComponent : ViewComponent
    {
        private readonly CmsShoppingCartContext context;

        public MainMenuViewComponent(CmsShoppingCartContext context)
        {
            this.context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var pages = await GetPageAsync();
            return View(pages);
        }

        private Task<List<Page>> GetPageAsync()
        {
            return context.Pages.OrderBy(p => p.Sorting).ToListAsync();
        }
    }
}

