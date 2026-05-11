using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartAuth.Domain.Users;

namespace SmartAuth.Infrastructure.Database.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles");
        builder.HasKey(x => x.Name);

        builder.Property(x => x.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasData(
            Role.Administrator,
            Role.Member
        );
    }
}
