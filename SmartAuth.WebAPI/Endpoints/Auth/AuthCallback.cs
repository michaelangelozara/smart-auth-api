using Microsoft.AspNetCore.Mvc;
using SmartAuth.WebAPI.Constants;

namespace SmartAuth.WebAPI.Endpoints.Auth;

public class AuthCallback : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/auth/callback", (string code) =>
        {
            return Results.Ok($"Successfully logged in with code {code}");
        })
        .WithTags(Tags.Authentication)
        .AllowAnonymous();
    }
}
