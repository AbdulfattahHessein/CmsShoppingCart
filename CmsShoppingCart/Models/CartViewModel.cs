using System.Collections.Generic;

namespace CmsShoppingCart.Models
{
    public class CartViewModel
    {
        public List<CartItem> CartItems;
        public decimal GrandTotal { get; set; }
    }
}
