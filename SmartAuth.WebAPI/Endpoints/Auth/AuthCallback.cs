using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartAuth.Application.Features.Auth.ExchangeCode;
using SmartAuth.SharedKernel;
using SmartAuth.WebAPI.Constants;
using SmartAuth.WebAPI.Extensions;
using SmartAuth.WebAPI.Infrastructure;

namespace SmartAuth.WebAPI.Endpoints.Auth;

public class AuthCallback : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/auth/callback", async (string code, ISender sender, HttpResponse httpResponse) =>
        {
            Result<Guid> result = await sender.Send(new ExchangeCodeCommand(code));

            return result.Match((id) =>
            {
                httpResponse.SetCookie("session_id", $"{id}", 168); // 7 days expiration

                return Results.Ok();
            }, CustomResults.Problem);
        })
        .WithTags(Tags.Authentication)
        .AllowAnonymous();
    }
}
