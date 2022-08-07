using Faith.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Faith.Persistence;

public class FaithDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public FaithDbContext(DbContextOptions<FaithDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FaithDbContext).Assembly);
    }
}