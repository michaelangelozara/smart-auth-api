namespace SmartAuth.Domain.Users;

public sealed class Permission
{
    public static readonly Permission GetUser = new("users:read");
    public static readonly Permission CreateUser = new("users:create");
    public static readonly Permission ModifyUser = new("users:modify");
    public static readonly Permission DeleteUser = new("users:delete");
    public static readonly Permission Submit = new("users:submit");

    private Permission(string name)
    {
        Name = name;
    }

    public string Name { get; private set; } = null!;
}
