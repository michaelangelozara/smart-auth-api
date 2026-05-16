using Microsoft.EntityFrameworkCore;
using SmartAuth.Domain.Users;

namespace SmartAuth.Infrastructure.Database.Repositories;

public sealed class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<User?> FindAsync(string identityId)
    {
        return await context.Users
            .Where(u => u.IdentityId == identityId)
            .FirstOrDefaultAsync();
    }

    public async Task InsertAsync(User user) 
        => await context.Users.AddAsync(user);
}