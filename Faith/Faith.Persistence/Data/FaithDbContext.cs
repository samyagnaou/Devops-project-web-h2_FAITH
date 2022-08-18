using Faith.Core.Models;
using Faith.Core.Models.Roles;
using Faith.Infrastructure.Data.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Faith.Infrastructure.Data
{
    public class FaithDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Mentor> Mentors => Set<Mentor>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<Message> Messages => Set<Message>();
        public DbSet<Comment> Comments => Set<Comment>();

        public FaithDbContext(DbContextOptions<FaithDbContext> options)
            : base(options)
        {
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.Database.EnsureCreated();
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new RoleConfiguration());
        }
    }
}

