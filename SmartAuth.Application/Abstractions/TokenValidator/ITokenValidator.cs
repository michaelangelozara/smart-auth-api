namespace SmartAuth.Application.Abstractions.TokenValidator;

public interface ITokenValidator
{
    Task<TokenValidationResult> ValidateAsync(string token, CancellationToken cancellationToken = default);
}
