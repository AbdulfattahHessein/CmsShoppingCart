using System.ComponentModel.DataAnnotations;

namespace CmsShoppingCart.Models.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [Required, MinLength(2, ErrorMessage = "Minimum length is 2")]
        [RegularExpression(@"^[a-zA-Z- ]+$", ErrorMessage = "Only letters, space and '-' are allowed")]
        public string Name { get; set; }
        public string Slug { get; set; } // url slug is unique for every page
        public int Sorting { get; set; }

    }
}
