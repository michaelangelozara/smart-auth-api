using SmartAuth.Domain.Exceptions;
using SmartAuth.SharedKernel;

namespace SmartAuth.Domain.Users;

public sealed class User
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
}
