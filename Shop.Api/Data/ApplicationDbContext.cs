using Microsoft.EntityFrameworkCore;
using Shop.Api.Models;

namespace Shop.Api.Data;

public class ApplicationDbContext:DbContext
{
    //никогда не надо делать несколько контекстов, если не нужно, особенно инжектить, еф сойдёт с ума в миграциях
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderProduct> OrderProducts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       base.OnModelCreating(modelBuilder);

       modelBuilder.Entity<User>()
           .HasMany(x => x.Orders)
           .WithOne(x => x.User)
           .HasForeignKey(x => x.UserId)
           .HasPrincipalKey(x => x.Id);
        
       modelBuilder.Entity<OrderProduct>()
           .HasKey(x => new { x.OrderId, x.ProductId });
    }

 
}