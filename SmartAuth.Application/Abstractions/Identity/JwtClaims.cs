using System.Text.Json.Serialization;

namespace SmartAuth.Application.Abstractions.Identity;

public sealed class JwtClaims
{
    [JsonPropertyName("sub")]
    public string IdentityId { get; init; } = default!;

    [JsonPropertyName("given_name")]
    public string FirstName { get; init; } = default!;

    [JsonPropertyName("family_name")]
    public string LastName { get; init; } = default!;

    [JsonPropertyName("email")]
    public string Email { get; init; } = default!;
}
