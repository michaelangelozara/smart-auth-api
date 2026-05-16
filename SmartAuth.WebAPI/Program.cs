using SmartAuth.Application;
using SmartAuth.Infrastructure;
using SmartAuth.Infrastructure.Extentions;
using SmartAuth.WebAPI;
using SmartAuth.WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services
    .AddWebAPI()
    .AddInfrastructure(builder.Configuration)
    .AddApplication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseScalar();

    app.UseMigration();
}


app.UseAuthentication();

app.UseAuthorization();

app.MapEndpoints();

app.Run();
