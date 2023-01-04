using Nmt.Domain.Enums;

namespace Nmt.Core.Auth;

public static class PermissionsHelper
{
    public static string ToPolicyName(Permission[] permissions)
    {
        return string.Join(',', permissions.Select(permission => (int)permission));
    }

    public static ISet<Permission> ToPermissionsSet(string policyName)
    {
        return policyName
            .Split(',')
            .Select(permission => (Permission)int.Parse(permission))
            .ToHashSet();
    }
}