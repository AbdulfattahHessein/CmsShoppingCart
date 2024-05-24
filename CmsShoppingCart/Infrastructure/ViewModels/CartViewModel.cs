using CmsShoppingCart.Models;
using System.Collections.Generic;

namespace CmsShoppingCart.Infrastructure.ViewModels
{
    public class CartViewModel
    {
        public List<CartItem> CartItems;
        public decimal GrandTotal { get; set; }
    }
}
