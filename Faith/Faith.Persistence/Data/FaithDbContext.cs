using Faith.Core.Models;
using Faith.Core.Models.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Faith.Infrastructure.Data
{
    public class FaithDbContext : IdentityDbContext<IdentityUser>
    {

        public FaithDbContext(DbContextOptions<FaithDbContext> options)
            : base(options)
        {
        }

        public DbSet<Mentor> Mentors => Set<Mentor>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<Message> Messages => Set<Message>();
        public DbSet<Comment> Comments => Set<Comment>();
    }
}

