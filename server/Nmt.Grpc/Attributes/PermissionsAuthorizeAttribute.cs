using Microsoft.AspNetCore.Authorization;
using Nmt.Core.Helpers;
using Nmt.Domain.Enums;

namespace Nmt.Grpc.Attributes;

public class PermissionsAuthorizeAttribute : AuthorizeAttribute
{
    public PermissionsAuthorizeAttribute(params Permission[] permissions)
    {
        Policy = PermissionsHelper.ToPolicyName(permissions);
    }
}