using System.ComponentModel.DataAnnotations;

namespace CmsShoppingCart.Models.Entities
{
    public class Page
    {
        public int Id { get; set; }

        [Required, MinLength(2, ErrorMessage = "Minimum length is 2")]
        [RegularExpression(@"^[a-zA-Z- ]+$", ErrorMessage = "Only letters, space and '-' are allowed")]
        public string Title { get; set; }
        public string Slug { get; set; }

        [Required, MinLength(4, ErrorMessage = "Minimum length is 4")]
        public string Content { get; set; }
        public int Sorting { get; set; }

    }
}
