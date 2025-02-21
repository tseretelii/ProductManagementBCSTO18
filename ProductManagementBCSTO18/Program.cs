using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductManagementBCSTO18.Handlers;
using ProductManagementBCSTO18.Interfaces;
using ProductManagementBCSTO18.Models;
using ProductManagementBCSTO18.Models.Entities;
using ProductManagementBCSTO18.Requiremenets;
using ProductManagementBCSTO18.Services;

namespace ProductManagementBCSTO18
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            //builder.Services.AddDefaultIdentity<User>().AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IAccauntService, AccauntService>();

            builder.Services.AddScoped<IAdminService, AdminService>();

            builder.Services.Configure<IdentityOptions>(x =>
            {
                x.Lockout.MaxFailedAccessAttempts = 3;
                x.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(10);
                x.User.RequireUniqueEmail = true;
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("FiveYearsEmployee", policy =>
                policy.Requirements.Add(new FiveYearsRequirement(5)));
            });

            builder.Services.AddScoped<IAuthorizationHandler, FiveYearsHandler>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
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

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
