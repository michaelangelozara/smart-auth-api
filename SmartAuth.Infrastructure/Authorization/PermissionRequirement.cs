using Microsoft.AspNetCore.Authorization;

namespace SmartAuth.Infrastructure.Authorization;

internal class PermissionRequirement(string permission)
    : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}
