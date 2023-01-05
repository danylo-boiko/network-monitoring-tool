using Microsoft.AspNetCore.Authorization;
using Nmt.Domain.Enums;

namespace Nmt.Core.Auth;

public class PermissionsRequirement : IAuthorizationRequirement
{
    public ISet<Permission> Permissions { get; }

    public PermissionsRequirement(string policyName)
    {
        Permissions = PermissionsHelper.ToPermissionsSet(policyName);
    }
}