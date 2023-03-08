using System.Collections.Generic;
using CmsShoppingCart.Models;

namespace CmsShoppingCart.Infrastructure.ViewModels
{
    public class CartViewModel
    {
        public List<CartItem> CartItems;
        public decimal GrandTotal { get; set; }
    }
}
