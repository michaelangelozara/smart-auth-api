using SmartAuth.Application.Abstractions.Authentication;
using SmartAuth.Infrastructure.Authentication;
using SmartAuth.Infrastructure.Authorization;
using SmartAuth.Infrastructure.Database;
using SmartAuth.Infrastructure.Time;
using SmartAuth.SharedKernel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartAuth.Domain.Users;
using SmartAuth.Infrastructure.Database.Repositories;
using SmartAuth.Application.Abstractions.Data;
using SmartAuth.Application.Abstractions.Identity;
using SmartAuth.Infrastructure.Identity;
using Microsoft.Extensions.Options;

namespace SmartAuth.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddServices()
            .AddDatabase(configuration)
            .AddHealthChecks(configuration)
            .AddAuthenticationInternal(configuration)
            .AddAuthorizationInternal()
            .AddRepositories()
            .AddOptions(configuration);

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddTransient<IIdentityProviderService, IdentityProviderService>();

        services.AddTransient<KeyCloakAuthDelegatingHandler>();

        services
            .AddHttpClient<KeyCloakClient>((sp, httpClient) =>
            {
                KeyCloakOptions keyCloakOptions = sp.GetRequiredService<IOptions<KeyCloakOptions>>().Value;
                
                httpClient.BaseAddress = new Uri(keyCloakOptions.AdminUrl);
            })
            .AddHttpMessageHandler<KeyCloakAuthDelegatingHandler>();

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        string? dbConnection = configuration.GetConnectionString("Database");
        if (dbConnection is null)
            throw new InvalidOperationException("Database connection string is not configured.");

        services.AddDbContext<AppDbContext>(options => 
            options.UseNpgsql(dbConnection)
                .UseSnakeCaseNamingConvention());

        return services;
    }

    private static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }

    private static IServiceCollection AddAuthenticationInternal(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication().AddJwtBearer();
        services.ConfigureOptions<JwtBearerConfigureOptions>();

        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContext>();

        
        return services;
    }

    private static IServiceCollection AddAuthorizationInternal(this IServiceCollection services)
    {
        services.AddAuthorization();

        // set all endpoints required authenticated user as default
        services.AddAuthorizationBuilder()
            .SetFallbackPolicy(new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build());

        services.AddScoped<PermissionProvider>();

        services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();

        services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());

        return services;
    }

    private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        IConfigurationSection keyCloakConfigurationSection = configuration.GetSection("KeyCloak");
        if (!keyCloakConfigurationSection.Exists())
        {
            throw new InvalidOperationException("Keycloak is not configured.");
        }

        services.Configure<KeyCloakOptions>(keyCloakConfigurationSection);

        return services;
    }
}
