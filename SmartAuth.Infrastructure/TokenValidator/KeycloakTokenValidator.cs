using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using SmartAuth.Application.Abstractions.TokenValidator;

namespace SmartAuth.Infrastructure.TokenValidator;

public sealed class KeycloakTokenValidator
    : ITokenValidator
{
    private readonly ConfigurationManager<OpenIdConnectConfiguration> _configurationManager;

    private readonly TokenValidationParameters _tokenValidationParameters;

    public KeycloakTokenValidator(IConfiguration configuration, IHostEnvironment env)
    {
        var metadataAddress = configuration["Authentication:MetadataAddress"]!;

        _configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
            metadataAddress,
            new OpenIdConnectConfigurationRetriever(),
            new HttpDocumentRetriever{ RequireHttps = !env.IsDevelopment() }
        );

        _tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = configuration["Keycloak:Issuer"],

            ValidateAudience = true,
            ValidAudience = configuration["Authentication:Audience"],

            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30),

            ValidateIssuerSigningKey = true
        };
    }

    public async Task<Application.Abstractions.TokenValidator.TokenValidationResult> ValidateAsync(string token, CancellationToken cancellationToken = default)
    {
        try
        {
            OpenIdConnectConfiguration discoveryDoc = await _configurationManager
                .GetConfigurationAsync(cancellationToken);

            var validationParams = _tokenValidationParameters.Clone();
            _tokenValidationParameters.IssuerSigningKeys = discoveryDoc.SigningKeys;

            var handler = new JsonWebTokenHandler();
            var result = await handler.ValidateTokenAsync(token, validationParams);

            if (result.IsValid)
                return new(true);

            if(result.Exception is SecurityTokenSignatureKeyNotFoundException)
            {
                _configurationManager.RequestRefresh();
                discoveryDoc = await _configurationManager.GetConfigurationAsync(cancellationToken);

                validationParams.IssuerSigningKeys = discoveryDoc.SigningKeys;
                result = await handler.ValidateTokenAsync(token, validationParams);

                return result.IsValid ? new(true) : new(false, result.Exception?.Message);
            }

            return new(false, result.Exception?.Message);
        }catch(Exception exception)
        {
            return new(false, exception.Message);
        }
    }
}