using MediatR;
using SmartAuth.Application.Features.Users.Register;
using SmartAuth.SharedKernel;
using SmartAuth.WebAPI.Constants;
using SmartAuth.WebAPI.Extensions;
using SmartAuth.WebAPI.Infrastructure;

namespace SmartAuth.WebAPI.Endpoints.Users;

public class RegisterUser : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/users", async (Request request, ISender sender, HttpResponse httpResponse) =>
        {
            Result<Guid> result = await sender.Send(new RegisterUserCommand(
                request.FirstName,
                request.MiddleName,
                request.LastName,
                request.Email,
                request.Email,
                request.Password));

            return result.Match(id =>
            {
                httpResponse.SetCookie("session_id", $"{id}", 168); // 7 days expiration

                return Results.Ok("User succesfully registered.");
            }, CustomResults.Problem);
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

    public string Email { get; init; } = null!;

    public string Password { get; init; } = null!;
}
