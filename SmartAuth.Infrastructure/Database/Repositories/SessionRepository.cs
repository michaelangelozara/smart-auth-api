using Microsoft.EntityFrameworkCore;
using SmartAuth.Domain.Sessions;

namespace SmartAuth.Infrastructure.Database.Repositories;

public class SessionRepository(AppDbContext context) : ISessionRepository
{
    public async Task<Session?> FindAsNoTrackingAsync(Guid sessionId)
    {
        return await context.Sessions
            .AsNoTracking()
            .Where(s => s.Id == sessionId)
            .FirstOrDefaultAsync();
    }
}