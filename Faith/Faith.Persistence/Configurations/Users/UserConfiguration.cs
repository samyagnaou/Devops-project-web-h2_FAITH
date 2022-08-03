using Faith.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Faith.Persistence.Configurations.Users;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.OwnsOne(o => o.Name, userName =>
        {
            userName.Property(d => d.Firstname).HasColumnName("Firstname").IsRequired();
            userName.Property(d => d.Lastname).HasColumnName("Lastname").IsRequired();
        });

        builder.Property(u => u.Gender).IsRequired();
        builder.Property(u => u.Email).HasMaxLength(128).IsRequired();
        builder.Property(u => u.DateOfBirth).IsRequired();
    }
}