namespace SmartAuth.Domain.Users;

public sealed class RolePermission
{
    private RolePermission()
    {
    }

    public string RoleName { get; private set; } = null!;
    public Role? Role { get; private set; }

    public string PermissionCode { get; private set; } = null!;
    public Permission? Permission { get; private set; }
}
