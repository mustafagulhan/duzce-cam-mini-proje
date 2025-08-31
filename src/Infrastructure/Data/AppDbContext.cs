using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Project> Projects => Set<Project>();
    public DbSet<TaskItem> TaskItems => Set<TaskItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Project>(builder =>
        {
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Description).HasMaxLength(500);
            builder.HasMany(p => p.Tasks)
                   .WithOne(t => t.Project)
                   .HasForeignKey(t => t.ProjectId)
                   .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<TaskItem>(builder =>
        {
            builder.Property(t => t.Title).IsRequired().HasMaxLength(100);
        });
    }
}


