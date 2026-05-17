namespace SmartAuth.Domain.Sessions;

public interface ISessionRepository
{
    Task<Session?> FindAsNoTrackingAsync(Guid sessionId);
}