using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartAuth.Domain.Users;

namespace SmartAuth.Infrastructure.Database.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FirstName)
            .HasColumnName("first_name")
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(x => x.MiddleName)
            .HasColumnName("middle_name")
            .HasMaxLength(100)
            .IsRequired(false);
        
        builder.Property(x => x.LastName)
            .HasColumnName("last_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.IdentityId)
            .HasColumnName("identity_id")
            .HasMaxLength(256)
            .IsRequired();

        builder.HasIndex(x => x.IdentityId)
            .IsUnique();
    }
}