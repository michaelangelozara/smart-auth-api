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

    public static Result<User> Create(string firstName, string? middleName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            return Result.Failure<User>(Error.Validation("Users.Validation", "First name cannot be null or empty."));
        }
        
        if (string.IsNullOrWhiteSpace(lastName))
        {
            return Result.Failure<User>(Error.Validation("Users.Validation", "Last name cannot be null or empty."));
        }
        
        var user = new User
        {
            FirstName = firstName,
            MiddleName = middleName,
            LastName = lastName
        };

        return Result.Success(user);
    }
}
