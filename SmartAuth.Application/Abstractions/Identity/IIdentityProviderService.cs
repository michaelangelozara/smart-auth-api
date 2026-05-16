using SmartAuth.SharedKernel;

namespace SmartAuth.Application.Abstractions.Identity;

public interface IIdentityProviderService
{
    Task<Result<string>> RegisterUserAsync(UserModel user, CancellationToken cancellationToken = default);

    Task<Result<TokenResponse>> ExchangeCodeAsync(string code, CancellationToken cancellationToken = default);

    JwtClaims ExtractClaim(string token);

    Task<TokenResponse> AuthenticateAsync(string username, string password,  CancellationToken cancellationToken = default);
}
