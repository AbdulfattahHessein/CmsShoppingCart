using CmsShoppingCart.Infrastructure;
using CmsShoppingCart.Infrastructure.ViewModels;
using CmsShoppingCart.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShoppingCart.Controllers
{
    public class CartController : Controller
    {
        private readonly CmsShoppingCartContext context;

        public CartController(CmsShoppingCartContext context)
        {
            this.context = context;
        }

        //GET /cart
        public IActionResult Index()
        {
            // this is a cart that have all items so it is a list of items that is make sense
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            CartViewModel cartViewModel = new CartViewModel()
            {
                CartItems = cart,
                GrandTotal = cart.Sum(c => c.Price * c.Quantity)
            };


            return View(cartViewModel);
        }

        //GET /cart/add/5
        public async Task<IActionResult> Add(int id)
        {
            var product = await context.Products.FindAsync(id); //search in cash first then in database

            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            CartItem cartItem = cart.FirstOrDefault(c => c.ProductId == id);

            if (cartItem == null)
            {
                cart.Add(new CartItem(product));
            }
            else
            {
                cartItem.Quantity++;
            }
            //CartViewModel cartViewModel = new CartViewModel()
            //{
            //    CartItems = cart,
            //    GrandTotal = cart.Sum(c => c.Price * c.Quantity)
            //};

            HttpContext.Session.SetJson("Cart", cart);

            if (HttpContext.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
                return RedirectToAction("Index");

            return ViewComponent("SmallCart");

        }

        //GET /cart/decrease/5
        public IActionResult Decrease(int id)
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");

            CartItem cartItem = cart.FirstOrDefault(c => c.ProductId == id);

            if (cartItem == null)
                return NotFound();

            if (cartItem.Quantity > 1)
            {
                cartItem.Quantity--;
            }
            else if (cartItem.Quantity == 1)
            {
                cart.RemoveAll(c => c.ProductId == id);
            }

            if (cart.Count == 0)
                HttpContext.Session.Remove("Cart");
            else
                HttpContext.Session.SetJson("Cart", cart);

            return RedirectToAction("Index");
        }

        //GET /cart/remove/5
        public IActionResult Remove(int id)
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");

            //CartItem cartItem = cart.Where(c => c.ProductId == id).FirstOrDefault();

            cart.RemoveAll(c => c.ProductId == id);

            if (cart.Count == 0)
                HttpContext.Session.Remove("Cart");
            else
                HttpContext.Session.SetJson("Cart", cart);

            return RedirectToAction("Index");
        }

        //GET /cart/clear/5
        public IActionResult Clear()
        {
            HttpContext.Session.Remove("Cart");
            //return RedirectToAction("Page", "Pages");
            //return Redirect("/");
            if (HttpContext.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
                return Redirect(Request.Headers["Referer"].ToString()); // redirect to the same page that made the request
            return Ok();
        }
    }
}
