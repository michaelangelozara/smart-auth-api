using SmartAuth.WebAPI.Extensions;
using SmartAuth.WebAPI.Infrastructure;
using System.Reflection;
using System.Text.Json;

namespace SmartAuth.WebAPI;

public static class DependencyInjection
{
    public static IServiceCollection AddWebAPI(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();

        // add all endpoints from this assembly
        services.AddEndpoints(Assembly.GetExecutingAssembly());

        services.ConfigureHttpJsonOptions(options =>
        {
            // make all json property naming convention to snake case
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
            options.SerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower;

            // add custom Date and Time format
            //options.SerializerOptions.Converters.Add(new CustomDateTimeJsonConverter("yyyy-MM-dd HH:mm:ss"));

            options.SerializerOptions.PropertyNameCaseInsensitive = true;
        });

        return services;
    }
}
