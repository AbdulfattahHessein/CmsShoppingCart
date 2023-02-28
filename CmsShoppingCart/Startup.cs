using CmsShoppingCart.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CmsShoppingCart
{
    public class Startup
    {
        #region Dependency Injection
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        #endregion

        #region Services
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region search about
            services.AddMemoryCache();
            services.AddSession(options =>
            {
                //it not used but it is exist and nice to know about it
                //options.IdleTimeout = TimeSpan.FromSeconds(2);
                //options.IdleTimeout = TimeSpan.FromDays(2);
            });
            #endregion

            services.AddControllersWithViews();

            #region Add DbContext service
            services.AddDbContext<CmsShoppingCartContext>(
                   options => options.UseSqlServer(Configuration.GetConnectionString("CmsShoppingCartContext"))
                   );
            #endregion
        }
        #endregion

        #region Configurations
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            #region Environment check 
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            #endregion

            app.UseHttpsRedirection();

            app.UseStaticFiles(); // to enable static file like in wwwroot

            app.UseRouting();

            #region search about
            app.UseSession();
            #endregion

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                #region Endpoints
                endpoints.MapControllerRoute(
                            name: "pages",
                            pattern: "{slug?}",
                            defaults: new { controller = "Pages", action = "Page" }
                            );
                endpoints.MapControllerRoute(
                   name: "products",
                   pattern: "products/{categorySlug}",
                   defaults: new { controller = "Products", action = "ProductsByCategory" }
                   );

                endpoints.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=pages}/{action=Index}/{id?}"
                );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                #endregion
            });
        }
        #endregion
    }
}
