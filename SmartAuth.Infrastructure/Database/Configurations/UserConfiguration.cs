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
        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.FirstName)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(x => x.MiddleName)
            .HasMaxLength(100)
            .IsRequired(false);
        
        builder.Property(x => x.LastName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.IdentityId)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(x => x.IdentityId)
            .IsUnique();
        builder.HasIndex(x => x.Email)
            .IsUnique();    
    }
}