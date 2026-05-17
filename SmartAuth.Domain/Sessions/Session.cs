using SmartAuth.Domain.Users;

namespace SmartAuth.Domain.Sessions;

public sealed class Session
{
    private Session()
    {
        
    }

    public Guid Id { get; private set; }

    public string AccessToken { get; private set; } = null!;

    public DateTime AccessTokenExpiration { get; private set; }

    public string RefreshToken { get; private set; } = null!;

    public DateTime RefreshTokenExpiration { get; private set; }

    public bool Revoked { get; private set; }

    public Guid UserId { get; private set; }
    public User? User { get; private set; }

    public static Session Create(
        string accessToken, 
        int accessTokenExpiration, 
        string refreshToken, 
        int refreshTokenExpiration,
        Guid userId)
    {
        return new Session
        {
          Id = Guid.NewGuid(),
          AccessToken = accessToken,
          AccessTokenExpiration = DateTime.UtcNow.AddSeconds(accessTokenExpiration),
          RefreshToken = refreshToken,
          RefreshTokenExpiration = DateTime.UtcNow.AddSeconds(refreshTokenExpiration),
          Revoked = false,
          UserId = userId
        };
    }

    public bool AccessTokenExpirationValid(DateTime now) => AccessTokenExpiration > now;
}
