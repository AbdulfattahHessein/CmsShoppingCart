using CmsShoppingCart.Infrastructure;
using CmsShoppingCart.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddMemoryCache();
builder.Services.AddSession(options =>
{
    // it not used but it is exist and nice to know about it
    // options.IdleTimeout = TimeSpan.FromSeconds(2);
    // options.IdleTimeout = TimeSpan.FromDays(2);
});

builder.Services.AddRouting(option => option.LowercaseUrls = true);
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<CmsShoppingCartContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("CmsShoppingCartContext"))
);

builder.Services.AddIdentity<AppUser, IdentityRole>(setupAction =>
{
    setupAction.Password.RequiredLength = 4;
    setupAction.Password.RequireNonAlphanumeric = false;
    setupAction.Password.RequireLowercase = false;
    setupAction.Password.RequireUppercase = false;
    setupAction.Password.RequireDigit = false;
})
.AddEntityFrameworkStores<CmsShoppingCartContext>()
.AddDefaultTokenProviders();

var app = builder.Build();

// Seed roles and admin user
using (var scope = app.Services.CreateAsyncScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<CmsShoppingCartContext>();

    try
    {
        await context.Database.MigrateAsync();

    }
    catch (Exception)
    {
        await context.Database.EnsureDeletedAsync();

        await context.Database.MigrateAsync();
    }

    await context.SeedDataAsync(services);
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{

    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Pages}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
       name: "default",
       pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
       name: "pages",
       pattern: "{slug?}",
       defaults: new { controller = "Pages", action = "Page" });

   
});
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Pages}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
