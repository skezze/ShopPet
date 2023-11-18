using Microsoft.EntityFrameworkCore;
using Shop.Api.Models;

namespace Shop.Api.Data;

public class UserDbContext:DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options):base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("user");
    }

    public DbSet<User> Users { get; set; }
}