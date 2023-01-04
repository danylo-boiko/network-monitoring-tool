using Microsoft.AspNetCore.Authorization;
using Nmt.Domain.Consts;
using Nmt.Domain.Enums;

namespace Nmt.Core.Auth;

public class PermissionsAuthorizationHandler : AuthorizationHandler<PermissionsRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionsRequirement requirement)
    {
        var userPermissions = context.User.Claims
            .Where(claim => claim.Type == AuthClaims.RoleClaim)
            .Select(claim => (Permission)int.Parse(claim.Value))
            .ToHashSet();

        if (userPermissions.Intersect(requirement.Permissions).Any())
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        context.Fail();
        return Task.CompletedTask;
    }
}