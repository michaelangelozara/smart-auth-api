using MediatR;
using SmartAuth.Application.Abstractions.Data;
using SmartAuth.Application.Abstractions.Identity;
using SmartAuth.Domain.Users;
using SmartAuth.SharedKernel;

namespace SmartAuth.Application.Features.Users.Register;

public class RegisterUserCommandHandler(
    IIdentityProviderService identityProviderService,
    IUnitOfWork unitOfWork,
    IUserRepository userRepository) 
    : IRequestHandler<RegisterUserCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        Result<string> result = await identityProviderService.RegisterUserAsync(
            new UserModel(
                request.Username,
                request.Email,
                request.Password,
                request.FirstName,
                request.LastName),
            cancellationToken);

        if (result.IsFailure)
        {
            return Result.Failure<Guid>(result.Error);
        }

        TokenResponse tokenResponse = await identityProviderService.AuthenticateAsync(request.Username, request.Password, cancellationToken);

        var user = User.Create(
            request.FirstName, 
            request.MiddleName, 
            request.LastName,
            request.Email,
            result.Value);

        Guid sessionId = user.AddSession(
            tokenResponse.AccessToken, 
            tokenResponse.ExpiresIn, 
            tokenResponse.RefreshToken, 
            tokenResponse.RefreshExpiresIn, 
            user.Id);

        await userRepository.InsertAsync(user);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return sessionId;
    }
}