using CmsShoppingCart.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace CmsShoppingCart.Infrastructure
{
    public class SmallCartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");
            SmallCartViewModel smallCartVM;

            if (cart == null)
            {
                smallCartVM = null;
            }
            else
            {
                smallCartVM = new SmallCartViewModel()
                {
                    NumberOfItems = cart.Sum(c => c.Quantity),
                    TotalAmount = cart.Sum(c => c.Quantity * c.Price)

                };
            }
            return View(smallCartVM);
        }
    }
}
