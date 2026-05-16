using MediatR;
using SmartAuth.SharedKernel;

namespace SmartAuth.Application.Features.Auth.ExchangeCode;

public sealed record ExchangeCodeCommand(string Code) : IRequest<Result<Guid>>;
