using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmartAuth.Infrastructure.Database;

namespace SmartAuth.Infrastructure.Extentions;

public static class WebApplicationExtensions
{
    public static void UseMigration(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        context.Database.Migrate();
    }
}
