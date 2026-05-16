namespace SmartAuth.Domain.Users;

public interface IUserRepository
{
    Task InsertAsync(User user);

    Task<User?> FindAsync(string identityId);
}