using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartAuth.Domain.Users;

namespace SmartAuth.Infrastructure.Database.Configurations;

public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable("role_permissions");
        builder.HasKey(x => new {x.RoleName, x.PermissionCode});

        builder.Property(x => x.RoleName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.PermissionCode)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasData(
            // Admin
            CreateRolePermission(Role.Administrator, Permission.GetUser),
            CreateRolePermission(Role.Administrator, Permission.CreateUser),
            CreateRolePermission(Role.Administrator, Permission.ModifyUser),
            CreateRolePermission(Role.Administrator, Permission.DeleteUser),
            CreateRolePermission(Role.Administrator, Permission.Submit),

            // Member
            CreateRolePermission(Role.Member, Permission.Submit)
        );

        builder.HasOne(rp => rp.Role)
            .WithMany()
            .HasForeignKey(ur => ur.RoleName)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasOne(rp => rp.Permission)
            .WithMany()
            .HasForeignKey(ur => ur.PermissionCode)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }

    private static object CreateRolePermission(Role role, Permission permission)
    {
        return new
        {
            RoleName = role.Name,
            PermissionCode = permission.Name
        };
    }
}
