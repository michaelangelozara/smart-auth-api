using MediatR;
using SmartAuth.SharedKernel;

namespace SmartAuth.Application.Features.Users;

public sealed record RegisterUserCommand(
    string FirstName,
    string? MiddleName,
    string LastName) : IRequest<Result<Guid>>;