using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyWebProject.Models;
using System.Reflection.Emit;

namespace MyWebProject.Date
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AllProduct>().HasKey(p => p.ProductId);
            modelBuilder.Entity<Category>().HasKey(p => p.CategoryId);
            modelBuilder.Entity<CartItem>().HasKey(c => c.CartId);
            modelBuilder.Entity<Order>().HasKey(p => p.OrderId);
            modelBuilder.Entity<SubCategory>().HasKey(p => p.SubCategoryId);
            base.OnModelCreating(modelBuilder);
        }

       
        public static async Task EnsureRolesCreated(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roleNames = { "Admin", "User" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }


        public DbSet<AllProduct> AllProducts { get; set; }
        public DbSet<Category> Categories { get; set; }
         public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Order> Orders { get; set; }

    }

}











