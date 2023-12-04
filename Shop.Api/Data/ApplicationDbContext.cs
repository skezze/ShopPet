using Microsoft.EntityFrameworkCore;
using Shop.Api.Models;

namespace Shop.Api.Data;

public class ApplicationDbContext:DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }

     protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }
}