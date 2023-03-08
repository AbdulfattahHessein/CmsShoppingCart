using System.ComponentModel.DataAnnotations;

namespace CmsShoppingCart.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, MinLength(2, ErrorMessage = "Minimum length is 2")]
        [RegularExpression(@"^[a-zA-Z]+$"), Display(Name = "Username")]
        public string UserName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password), Required, MinLength(8, ErrorMessage = "Minimum length is 8")]
        public string Password { get; set; }

    }
}
