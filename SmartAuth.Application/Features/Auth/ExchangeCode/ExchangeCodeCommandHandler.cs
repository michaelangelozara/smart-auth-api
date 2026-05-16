using MediatR;
using SmartAuth.Application.Abstractions.Data;
using SmartAuth.Application.Abstractions.Identity;
using SmartAuth.Domain.Users;
using SmartAuth.SharedKernel;

namespace SmartAuth.Application.Features.Auth.ExchangeCode;

public class ExchangeCodeCommandHandler(
    IIdentityProviderService identityProviderService,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<ExchangeCodeCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(ExchangeCodeCommand request, CancellationToken cancellationToken)
    {
        Result<TokenResponse> identityResult = await identityProviderService.ExchangeCodeAsync(request.Code, cancellationToken);

        if (identityResult.IsFailure)
        {
            return Result.Failure<Guid>(identityResult.Error);
        }

        TokenResponse tokenResponse = identityResult.Value;

        JwtClaims claims = identityProviderService.ExtractClaim(tokenResponse.AccessToken);

        User? user = await userRepository.FindAsync(claims.IdentityId);
        
        if(user is null)
        {
            user = User.Create(
                claims.FirstName, 
                "", 
                claims.LastName,
                claims.Email,
                claims.IdentityId);

            await userRepository.InsertAsync(user);
        }

         Guid sessionId = user.AddSession(
                tokenResponse.AccessToken, 
                tokenResponse.ExpiresIn,
                tokenResponse.RefreshToken, 
                tokenResponse.RefreshExpiresIn, 
                user.Id);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return sessionId;
    }
}
