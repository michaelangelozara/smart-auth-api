using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SmartAuth.Application.Abstractions.Identity;
using SmartAuth.Infrastructure.Extentions;
using SmartAuth.SharedKernel;

namespace SmartAuth.Infrastructure.Identity;

public sealed class IdentityProviderService(
    KeyCloakClient keyCloakClient,
    ILogger<IdentityProviderService> logger,
    HttpClient httpClient,
    IOptions<KeyCloakOptions> options) : IIdentityProviderService
{
    private const string PasswordCredentialType = "Password";
    private readonly KeyCloakOptions keyCloakOptions = options.Value;

    public async Task<TokenResponse> AuthenticateAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        var formParams = new Dictionary<string, string>
        {
            ["grant_type"] = "password",
            ["client_id"] = keyCloakOptions.PublicClientId,
            ["username"] = username,
            ["password"] = password
        };

        HttpResponseMessage response = await httpClient.PostAsync(
            keyCloakOptions.TokenUrl,
            new FormUrlEncodedContent(formParams),
            cancellationToken
        );

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException("Keycloak authentication failed.");
        }

        string json = await response.Content.ReadAsStringAsync(cancellationToken);
        TokenResponse tokens = JsonSerializer.Deserialize<TokenResponse>(json)!;

        return tokens;
    }

    public async Task<Result<TokenResponse>> ExchangeCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return Result.Failure<TokenResponse>(IdentityProviderErrors.CodeIsNullOrEmpty);
        }

        var formParams = new Dictionary<string, string>
        {
            ["grant_type"]   = "authorization_code",
            ["client_id"]    = keyCloakOptions.PublicClientId,
            ["code"]         = code,
            ["redirect_uri"] = keyCloakOptions.RedirectUri  
        };

        HttpResponseMessage response = await httpClient.PostAsync(
            keyCloakOptions.TokenUrl, 
            new FormUrlEncodedContent(formParams), 
            cancellationToken);

        if(!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException("Keycloak exchange code failed.");
        }

        string json = await response.Content.ReadAsStringAsync(cancellationToken);
        TokenResponse tokens = JsonSerializer.Deserialize<TokenResponse>(json)!;

        return tokens;
    }

    public JwtClaims ExtractClaim(string token)
    {
        var handler = new JwtSecurityTokenHandler();

        JwtSecurityToken jwt = handler.ReadJwtToken(token);

        var claims = new JwtClaims
        {
            IdentityId = jwt.Claims.GetValue("sub")!,
            FirstName = jwt.Claims.GetValue("given_name")!,
            LastName = jwt.Claims.GetValue("family_name")!,
            Email = jwt.Claims.GetValue("email")!
        };

        return claims;        
    }

    public async Task<Result<string>> RegisterUserAsync(UserModel user, CancellationToken cancellationToken = default)
    {
        var userRepresentation = new UserRepresentation(
            user.Username,
            user.Email,
            user.FirstName,
            user.LastName,
            true,
            true,
            [new CredentialRepresentation(PasswordCredentialType, user.Password, false)]);

        try
        {
            string identityId = await keyCloakClient.RegisterUserAsync(userRepresentation, cancellationToken);
            
            return identityId;
        }catch(HttpRequestException exception) when(exception.StatusCode == HttpStatusCode.Conflict)
        {
            logger.LogError(exception, "User registration failed");

            return Result.Failure<string>(IdentityProviderErrors.EmailIsNotUnique);
        }
    }
}
