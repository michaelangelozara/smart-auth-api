using MediatR;
using SmartAuth.Application.Abstractions.Data;
using SmartAuth.Application.Abstractions.Identity;
using SmartAuth.Domain.Users;
using SmartAuth.SharedKernel;

namespace SmartAuth.Application.Features.Auth.ExchangeCode;

public class ExchangeCodeCommandHandler(
    IIdentityProviderService identityProviderService,
    // ISessionRepository sessionRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<ExchangeCodeCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(ExchangeCodeCommand request, CancellationToken cancellationToken)
    {
        var identityResult = await identityProviderService.ExchangeCodeAsync(request.Code, cancellationToken);

        if (identityResult.IsFailure)
        {
            return Result.Failure<Guid>(identityResult.Error);
        }

        var tokenResponse = identityResult.Value;

        var claims = identityProviderService.ExtractClaim(tokenResponse.AccessToken);

        var user = await userRepository.FindAsync(claims.IdentityId);
    

        // await sessionRepository.InsertAsync(session);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Guid.NewGuid();
    }
}
