using BLL.Interfaces;
using BLL.Repositories;
using DAL.Contexts;
using DAL.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PL.ModelsProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PL
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
            services.AddControllersWithViews();
            services.AddDbContext<MvcAppDbContext>(option =>
            {
                option.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            }); // Allow Dependency Injection

            services.AddScoped<IDepartmentRepository, DepartmentRepository>(); // Allow Dependency injection for Class DepartmentRepository
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddAutoMapper(C => C.AddProfile(new EmployeeProfile()));

            // Add / Implement Interfaces and Class that Every Service depend on 

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // Add some Configurations => Option

                options.Password.RequireNonAlphanumeric = true; // @#$
                options.Password.RequireUppercase = true; // AUC
                options.Password.RequireLowercase = true; // auc
                options.Password.RequireDigit = true;     // 123 



            }) // this Add Interfaces
                .AddEntityFrameworkStores<MvcAppDbContext>() //this  Add Classes these implement the interfaces
                .AddDefaultTokenProviders(); // Add the rest of Classes that Generate [TOKEN] for => PasswordSignInAsync Fun()




            // services.AddScoped<UserManager<ApplicationUser>>();
            // Important note instead of allowing Dependency Injection For
            // three Services [UserManager,SignIn Manager,Role Manager] 
            // Use the Built In Function => AddAuthentication() =>  Recommended
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(Options =>
            {
                // Configurations
                Options.LoginPath = "Account/Login"; 
                Options.AccessDeniedPath = "Home/Error";

            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}/{id?}");
            });
        }
    }
}
