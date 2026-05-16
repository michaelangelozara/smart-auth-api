using System;
using System.Security.Claims;

namespace SmartAuth.Infrastructure.Extentions;

public static class ClaimExtensions
{
    public static string? GetValue(
        this IEnumerable<Claim> claims,
        string type)
    {
        return claims
            .FirstOrDefault(x => x.Type == type)
            ?.Value;
    }
}
