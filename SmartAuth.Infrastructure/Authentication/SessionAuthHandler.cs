using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SmartAuth.Application.Abstractions.TokenValidator;
using SmartAuth.Domain.Sessions;
using SmartAuth.SharedKernel;

namespace SmartAuth.Infrastructure.Authentication;

public class SessionAuthHandler(
    IOptionsMonitor<SessionAuthOptions> optionsMonitor,
    ILoggerFactory logger,
    UrlEncoder urlEncoder,
    IDateTimeProvider dateTimeProvider,
    ISessionRepository sessionRepository,
    ITokenValidator tokenValidator)
    : AuthenticationHandler<SessionAuthOptions>(optionsMonitor, logger, urlEncoder)
{
    protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // skip immediately if endpoint is marked as AllowAnonymous
        if(Context.GetEndpoint()?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            return AuthenticateResult.NoResult();
        
        if (!Request.Cookies.TryGetValue(Options.CookieName, out var rawSessionId)
            || !Guid.TryParse(rawSessionId, out var sessionId))
            return AuthenticateResult.NoResult();

        Session? session = await sessionRepository.FindAsNoTrackingAsync(sessionId);
        if(session is null || session.Revoked)
            return AuthenticateResult.Fail("Session not found or revoked.");

        if (!session.AccessTokenExpirationValid(dateTimeProvider.UtcNow))
                return AuthenticateResult.Fail("Session expired.");

        TokenValidationResult tokenValidationResult = await tokenValidator.ValidateAsync(
            session.AccessToken, 
            Context.RequestAborted);

        if (!tokenValidationResult.IsValid)
        {
            Logger.LogWarning(
                "Access token invalid for session {SessionId}: {Reason}",
                sessionId, tokenValidationResult.Reason);

            return AuthenticateResult.Fail($"Access token invalid: {tokenValidationResult.Reason}");
        }

        var claims = new Claim[]
        {
            new(ClaimTypes.NameIdentifier, session.UserId.ToString())
        };

        var identity = new ClaimsIdentity(
            claims,
            authenticationType: Scheme.Name,
            nameType: ClaimTypes.Name,   
            roleType: ClaimTypes.Role
        );

        var principal = new ClaimsPrincipal(identity);
        var ticket    = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
}
