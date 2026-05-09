using Scalar.AspNetCore;

namespace SmartAuth.WebAPI.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseScalar(this WebApplication app)
    {
        app.MapOpenApi().AllowAnonymous();
        app.MapScalarApiReference().AllowAnonymous();

        return app;
    }
}
