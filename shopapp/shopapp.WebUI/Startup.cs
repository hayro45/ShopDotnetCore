using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using shopapp.data.Concrete.EfCore;
using shopapp.data.Abstract;
using shopapp.business.Abstract;
using shopapp.business.Concrete;
using Microsoft.EntityFrameworkCore;
using shopapp.WebUI.Identity;
using Microsoft.AspNetCore.Identity;
using shopapp.WebUI.EmailServices;
using Microsoft.Extensions.Configuration;
using shopapp.data.Concrete;

namespace shopapp.WebUI
{
    public class Startup
    {
        private  IConfiguration _configuiration;
        public Startup(IConfiguration configuiration)
        {
            this._configuiration = configuiration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options=>options.UseSqlite("Data Source=shopDb"));
            services.AddIdentity<User,IdentityRole>().AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();
            
            services.Configure<IdentityOptions>(options =>{
                //password
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase= true;
                options.Password.RequireUppercase= true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = true;

                //lockout
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.AllowedForNewUsers = true;

                // options.User.AllowedUserNameCharacters = "";
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;

            });

            services.ConfigureApplicationCookie(options => {
                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/logout";
                options.AccessDeniedPath = "/account/accessdenied";
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.Cookie = new CookieBuilder{
                    HttpOnly = true,
                    Name = ".ShopApp.Security.Cookie",
                    SameSite = SameSiteMode.Strict
                };
            });

            services.AddScoped<IProductRepository, EfCoreProductRepository>();
            services.AddScoped<ICategoryRepository, EfCoreCategoryRepository>();
            services.AddScoped<ICartRepository, EfCoreCartRepository>();
            services.AddScoped<IOrderRepository, EfCoreOrderRepository>();

            services.AddScoped<IProductService, ProductManager>();
            services.AddScoped<ICategoryService, CategoryManager>();
            services.AddScoped<ICartService, CartManager>();
            services.AddScoped<IOrderService, OrderManager>();
            
            services.AddScoped<IMailSender, SmtpEmailSender>(i=>
                    new SmtpEmailSender(
                            _configuiration["EmailSender:Host"],
                            _configuiration.GetValue<int>("EmailSender:Port"),
                            _configuiration.GetValue<bool>("EmailSender:EnableSSL"),
                            _configuiration["EmailSender:UserName"],    
                            _configuiration["EmailSender:Password"]
                        ));

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {

            app.UseStaticFiles();
            app.UseStaticFiles( new StaticFileOptions{
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "node_modules")),
                    RequestPath= "/modules"
            });
            if (env.IsDevelopment())
            {
                SeedDatabase.Seed();
                app.UseDeveloperExceptionPage();
            }
             
            

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                //Cart routes
                endpoints.MapControllerRoute(
                    name: "cart", 
                    pattern: "cart",
                    defaults: new {controller="Cart", action="Index"}
                );
                endpoints.MapControllerRoute(
                    name: "cartcheckout", 
                    pattern: "checkout",
                    defaults: new {controller="Cart", action="Checkout"}
                );


                //user routes
                endpoints.MapControllerRoute(
                    name: "adminusers", 
                    pattern: "admin/user/list",
                    defaults: new {controller="Admin", action="UserList"}
                );

                endpoints.MapControllerRoute(
                    name: "adminuseredit", 
                    pattern: "admin/user/{id?}",
                    defaults: new {controller="Admin", action="UserEdit"}
                );

                //roleroutes

                endpoints.MapControllerRoute(
                    name: "adminroles", 
                    pattern: "admin/role/list",
                    defaults: new {controller="Admin", action="RoleList"}
                );

                endpoints.MapControllerRoute(
                    name: "adminrolecreate", 
                    pattern: "admin/role/create",
                    defaults: new {controller="Admin", action="RoleCreate"}
                );

                endpoints.MapControllerRoute(
                    name: "adminroleedit", 
                    pattern: "admin/role/{id?}",
                    defaults: new {controller="Admin", action="RoleEdit"}
                );


                //product route

                endpoints.MapControllerRoute(
                    name: "adminproducts", 
                    pattern: "admin/products",
                    defaults: new {controller="Admin", action="ProductList"}
                );
                endpoints.MapControllerRoute(
                    name: "adminproductcreate", 
                    pattern: "admin/products/create",
                    defaults: new {controller="Admin", action="CreateProduct"}
                );
                endpoints.MapControllerRoute(
                    name: "adminproductedit", 
                    pattern: "admin/products/{id?}",
                    defaults: new {controller="Admin", action="ProductEdit"}
                );

                endpoints.MapControllerRoute(
                    name: "admincategories", 
                    pattern: "admin/categories",
                    defaults: new {controller="Admin", action="CategoryList"}
                );

                endpoints.MapControllerRoute(
                    name: "admincategorycreate", 
                    pattern: "admin/categories/create",
                    defaults: new {controller="Admin", action="CreateCategory"}
                );
                endpoints.MapControllerRoute(
                    name: "admincategoryedit", 
                    pattern: "admin/categories/{id?}",
                    defaults: new {controller="Admin", action="CategoryEdit"}
                );

                // localhost/search
                endpoints.MapControllerRoute(
                    name: "search", 
                    pattern: "search",
                    defaults: new {controller="Shop", action="search"}
                );

                endpoints.MapControllerRoute(
                    name: "productdetails", 
                    pattern: "{url}",
                    defaults: new {controller="Shop",action="details"}
                );

                endpoints.MapControllerRoute(
                    name: "products", 
                    pattern: "products/{category?}",
                    defaults: new {controller="Shop",action="list"}
                );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern:"{controller=Home}/{action=Index}/{id?}"
                );
            });

            SeedIdentity.Seed(userManager, roleManager, configuration).Wait();
        }
    }
}
