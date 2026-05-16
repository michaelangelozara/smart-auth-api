using SmartAuth.Domain.Sessions;
using SmartAuth.SharedKernel;

namespace SmartAuth.Domain.Users;

public sealed class User : Entity
{
    private User()
    {
    }
    
    public Guid Id { get; private set; }
    
    public string FirstName { get; private set; } = null!;
    
    public string? MiddleName { get; private set; }
    
    public string LastName { get; private set; } = null!;

    public string Email { get; private set; } = null!;

    public string IdentityId { get; private set; } = null!;

    public ICollection<Session> Sessions { get; private set; } = [];

    public static User Create(string firstName, string? middleName, string lastName, string email, string identityId)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            FirstName = firstName,
            MiddleName = middleName,
            LastName = lastName,
            Email = email,
            IdentityId = identityId
        };
    }

    public Guid AddSession(
        string accessToken,
        int accessTokenExpiration,
        string refreshToken,
        int refreshTokenExpiration,
        Guid userId)
    {
        var session = Session.Create(
            accessToken, 
            accessTokenExpiration, 
            refreshToken, 
            refreshTokenExpiration, 
            userId);
        
        Sessions.Add(session);

        return session.Id;
    }
}
