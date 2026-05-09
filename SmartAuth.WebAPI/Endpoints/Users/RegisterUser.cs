using SmartAuth.WebAPI.Constants;

namespace SmartAuth.WebAPI.Endpoints.Users;

public class RegisterUser : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/users", (Request request) =>
        {
            return Results.Ok(request);
        })
            .WithTags(Tags.Users)
            .AllowAnonymous();
    }
}

internal sealed class Request
{
    public string FirstName { get; init; } = null!;
    
    public string? MiddleName { get; init; }
    
    public string LastName { get; init; } = null!;
}
