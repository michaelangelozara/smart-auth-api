using SmartAuth.Domain.Users;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using SmartAuth.Application.Abstractions.Data;
using SmartAuth.Domain.Sessions;

namespace SmartAuth.Infrastructure.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options), IUnitOfWork
{
    public DbSet<User> Users { get; private set; }

    public DbSet<Role> Roles { get; private set; }

    public DbSet<Permission> Permissions { get; private set; }

    public DbSet<UserRole> UserRoles { get; private set; }

    public DbSet<RolePermission> RolePermissions { get; private set; }

    public DbSet<Session> Sessions { get; private set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // apply all derived classes of IEntityTypeConfiguration
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
