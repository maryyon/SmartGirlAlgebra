using SmartGirlAlgebra.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SmartGirlAlgebra.API.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<UserProgress> UserProgress { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // Configure UserProgress
        builder.Entity<UserProgress>()
            .HasOne(p => p.User)
            .WithOne(u => u.Progress)
            .HasForeignKey<UserProgress>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<UserProgress>()
            .HasIndex(p => p.UserId)
            .IsUnique();
    }
}

