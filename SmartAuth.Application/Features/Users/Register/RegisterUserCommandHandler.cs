using MediatR;
using SmartAuth.Application.Abstractions.Data;
using SmartAuth.Domain.Users;
using SmartAuth.SharedKernel;

namespace SmartAuth.Application.Features.Users.Register;

public class RegisterUserCommandHandler(
    IUnitOfWork unitOfWork,
    IUserRepository userRepository) 
    : IRequestHandler<RegisterUserCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var userResult = User.Create(
            request.FirstName, 
            request.MiddleName, 
            request.LastName, 
            "temp_identity");

        if (userResult.IsFailure)
        {
            return Result.Failure<Guid>(userResult.Error);
        }

        await userRepository.InsertAsync(userResult.Value);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(userResult.Value.Id);
    }
}