using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShopApp;
using WebShopApp.Infrastructure.Data.Entities;

namespace WebShopApp.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Brand> Brands { get; set; }    
    public DbSet<Product> Products { get;  set; }    
    public DbSet<Category> Categories { get;  set; } 
    public DbSet<Order> Orders { get; set; }  
}
