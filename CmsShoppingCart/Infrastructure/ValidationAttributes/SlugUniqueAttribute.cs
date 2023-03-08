using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CmsShoppingCart.Infrastructure.ValidationAttributes
{
    public class SlugUniqueAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var context = validationContext.GetService(typeof(CmsShoppingCartContext)) as CmsShoppingCartContext;

            var slug = (string)value;

            // get all pages but not the edit category and the check if there is another category with the same slug name of edit category 
            var slugCount = context.Pages.Where(x => x.Slug.ToLower() == slug.ToLower()).Count();
            if (slugCount == 1)
                return ValidationResult.Success;
            return new ValidationResult("The category is already exist");

        }
    }
}
