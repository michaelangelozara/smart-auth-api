namespace SmartAuth.Infrastructure.Identity;

public sealed record UserRepresentation(
    string Username,
    string Email,
    string FirstName,
    string LastName,
    bool EmailVerified,
    bool Enabled,
    CredentialRepresentation[] Credentials
);
