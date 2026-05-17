namespace SmartAuth.Application.Abstractions.TokenValidator;

public sealed record TokenValidationResult(bool IsValid, string? Reason = null);