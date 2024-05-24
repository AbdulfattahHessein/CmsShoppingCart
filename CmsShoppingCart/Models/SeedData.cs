using CmsShoppingCart.Infrastructure;
using CmsShoppingCart.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace CmsShoppingCart.Models
{
    public static class SeedData
    {
        public static async Task SeedDataAsync(this CmsShoppingCartContext context, IServiceProvider serviceProvider)
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            string[] roles = { "admin", "editor" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var adminUser = new AppUser
            {
                UserName = configuration["AdminUser:UserName"],
                Email = configuration["AdminUser:Email"]
            };

            string adminPassword = configuration["AdminUser:Password"];

            var user = await userManager.FindByEmailAsync(adminUser.Email);

            if (user == null)
            {
                var createAdminUser = await userManager.CreateAsync(adminUser, adminPassword);
                if (createAdminUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "admin");
                }
            }

            if (await context.Pages.AnyAsync())
            {
                return;
            }

            await context.Pages.AddRangeAsync(
                new Page
                {
                    Title = "Home",
                    Slug = "home",
                    Content = "home page",
                    Sorting = 0
                },
                new Page
                {
                    Title = "About Us",
                    Slug = "about-us",
                    Content = "about us page",
                    Sorting = 0
                },
                new Page
                {
                    Title = "Services",
                    Slug = "services",
                    Content = "services page",
                    Sorting = 100
                },
                 new Page
                 {
                     Title = "Contact",
                     Slug = "contact",
                     Content = "contact page",
                     Sorting = 100
                 }
           );
            await context.SaveChangesAsync();
        }
    }
}
