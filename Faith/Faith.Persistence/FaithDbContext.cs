using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Faith.Domain.Posts;
using Faith.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Faith.Persistence
{
    public class FaithDbContext : DbContext
    {
        public DbSet<Post> Artworks { get; set; }
        public DbSet<User> Users { get; set; }


        public FaithDbContext(DbContextOptions<FaithDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FaithDbContext).Assembly);
        }

    }
}
