using HotChocolate.AspNetCore.Authorization;
using Nmt.Domain.Enums;

namespace Nmt.GraphQL.Attributes;

public class PermissionsAuthorizeAttribute : AuthorizeAttribute
{
    public PermissionsAuthorizeAttribute(params Permission[] permissions)
    {
        Policy = string.Join(',', permissions.Select(permission => (int)permission));
    }
}