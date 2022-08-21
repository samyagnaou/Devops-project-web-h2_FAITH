using Faith.Infrastructure.Data.Configurations;
using Faith.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Faith.Infrastructure.Data
{
    public class FaithPlatformContext : IdentityDbContext<IdentityUser>
    {
        public FaithPlatformContext(DbContextOptions<FaithPlatformContext> options)
            : base(options)
        {

        }

        public DbSet<Mentor> Mentors => Set<Mentor>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<Message> Messages => Set<Message>();
        public DbSet<Comment> Comments => Set<Comment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           // modelBuilder.ApplyConfiguration(new RoleConfiguration());
        }
    }
}