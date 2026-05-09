using SmartAuth.Domain.Users;

namespace SmartAuth.Infrastructure.Database.Repositories;

public sealed class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task InsertAsync(User user) 
        => await context.Users.AddAsync(user);
}