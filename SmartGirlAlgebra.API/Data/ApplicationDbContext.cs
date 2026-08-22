using Microsoft.EntityFrameworkCore;
using SmartGirlAlgebra.API.Models;

namespace SmartGirlAlgebra.API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Player> Players => Set<Player>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Player>()
            .HasIndex(p => p.Code)
            .IsUnique();
    }
}
