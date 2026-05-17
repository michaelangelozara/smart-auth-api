using Microsoft.AspNetCore.Authentication;

namespace SmartAuth.Infrastructure.Authentication;

public sealed class SessionAuthOptions : AuthenticationSchemeOptions
{
    public string CookieName { get; set; } = "session_id";

    public TimeSpan ClockSkew { get; set; } = TimeSpan.FromSeconds(30);

    public bool SlidingExpiration { get; set; } = false;
}