using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace CmsShoppingCart.Infrastructure.ValidationAttributes
{
    public class FileExtensionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //used to get the context service from ValidationContext
            //var context = (CmsShoppingCartContext)validationContext.GetService(typeof(CmsShoppingCartContext));

            var file = value as IFormFile;
            if (file != null)
            {
                var extention = Path.GetExtension(file.FileName);
                string[] extentions = { "jpg", "png" };
                bool result = extentions.Any(x => extention.EndsWith(x));

                if (!result)
                    return new ValidationResult(GetErrorMessage());
            }
            return ValidationResult.Success;


        }
        private string GetErrorMessage() => "Allowed extensions are jpg or png";

    }
}