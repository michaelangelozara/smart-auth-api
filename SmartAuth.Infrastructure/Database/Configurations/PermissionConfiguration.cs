using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartAuth.Domain.Users;

namespace SmartAuth.Infrastructure.Database.Configurations;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("permissions");
        builder.HasKey(x => x.Name);

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(50)
            .IsRequired();

        builder.HasData(
            Permission.GetUser,
            Permission.CreateUser,
            Permission.ModifyUser,
            Permission.DeleteUser,
            Permission.Submit
        );
    }
}
