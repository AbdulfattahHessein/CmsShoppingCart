﻿using CmsShoppingCart.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace CmsShoppingCart.Models
{
    public class SeedData
    {
        public static void Initailize(IServiceProvider serverProvider)
        {
            using (var context = new CmsShoppingCartContext
                (serverProvider.GetRequiredService<DbContextOptions<CmsShoppingCartContext>>()))
            {
                if (context.Pages.Any())
                {
                    return;
                }
                context.Pages.AddRange(
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
                context.SaveChanges();

            }
        }
    }
}