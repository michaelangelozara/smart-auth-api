namespace SmartAuth.Domain.Sessions;

public interface ISessionRepository
{
    Task InsertAsync(Session session);
}