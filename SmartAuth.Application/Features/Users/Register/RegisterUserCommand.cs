using MediatR;
using SmartAuth.SharedKernel;

namespace SmartAuth.Application.Features.Users.Register;

public sealed record RegisterUserCommand(
    string FirstName,
    string? MiddleName,
    string LastName,
    string Username,
    string Email,
    string Password) : IRequest<Result<Guid>>;