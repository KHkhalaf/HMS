using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using HMS.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HMS.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HMS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<AccountViewModel, AppRole>(options => {
                options.User.RequireUniqueEmail = true; 
                options.SignIn.RequireConfirmedAccount = true;
            }).AddEntityFrameworkStores<ApplicationDbContext>();
 
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider services)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
            CreateRoles(services).Wait();
        }
        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<AccountViewModel>>();

            IdentityResult roleResult;
            //here in this line we are adding Admin Role 
            string[] roleNames = { "Admin", "Staff", "Customer" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    var role = new AppRole();
                    role.Name = roleName;
                    roleResult = await RoleManager.CreateAsync(role);
                }
            }
            //here we are assigning the Admin role to the User that we have registered above 
            //Now, we are assinging admin role to this user("Khalil@gmail.com"). When will we run this project then it will
            //be assigned to that user.
            var userCheck = await UserManager.FindByEmailAsync("kalilAdmin@gmail.com");
            if (userCheck == null)
            {
                AccountViewModel userAdmin = new AccountViewModel();
                userAdmin.UserName = "khalil";
                userAdmin.Email = "khalilAdmin@gmail.com";
                userAdmin.Password = "AS";
                userAdmin.ConfirmPassword = "AS";
                userAdmin.City = "Damascus";
                userAdmin.PhoneNumber = "543210";
                userAdmin.SecurityStamp = Guid.NewGuid().ToString();
                var _user = await UserManager.FindByEmailAsync(userAdmin.Email);

                if (_user == null)
                {
                    var createuserAdmin = await UserManager.CreateAsync(userAdmin);
                    if (createuserAdmin.Succeeded)
                    {
                        //here we tie the new user to the role
                        await UserManager.AddToRoleAsync(userAdmin, "Admin");

                    }
                }
            }
        }
    }
}
