using SmartAuth.SharedKernel;

namespace SmartAuth.Application.Abstractions.Identity;

public static class IdentityProviderErrors
{
    public static readonly Error EmailIsNotUnique = Error.Conflict(
        "Identity.EmailIsNotUnique", 
        "The specified email is not unique.");

    public static readonly Error CodeIsNullOrEmpty = Error.Validation(
        "Identity.CodeIsNullOrEmpty", 
        "Authorization code cannot be null or empty.");

    public static readonly Error CodeIsInvalid = Error.Problem(
        "Identity.CodeIsInvalid", 
        "Authorization code is invalid.");

    public static readonly Error IdentityIdCannotExtractFromToken = Error.Problem(
        "Identity.IdentityIdCannotExtractFromToken", 
        "Identity id cannot extract from the provided token.");

    public static readonly Error InvalidUserCredential = Error.Problem(
        "Identity.InvalidUserCredential", 
        "Incorrect username or passsword.");
}