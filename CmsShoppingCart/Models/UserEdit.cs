using System.ComponentModel.DataAnnotations;

namespace CmsShoppingCart.Models
{
    public class UserEdit
    {

        [Required, EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password), MinLength(8, ErrorMessage = "Minimum length is 8")]
        public string Password { get; set; }



        public UserEdit() { }
        public UserEdit(AppUser user)
        {
            Email = user.Email;
            Password = user.PasswordHash;

        }
    }
}
