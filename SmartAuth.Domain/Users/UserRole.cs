namespace SmartAuth.Domain.Users;

public sealed class UserRole
{
    private UserRole()
    {
    }

    public Guid UserId { get; private set; }
    public User? User { get; private set; }

    public string RoleName { get; private set; } = null!;
    public Role? Role { get; private set; }
}
