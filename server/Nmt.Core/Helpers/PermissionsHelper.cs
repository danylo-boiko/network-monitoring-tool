using Nmt.Domain.Enums;

namespace Nmt.Core.Helpers;

public static class PermissionsHelper
{
    public static string ToPolicyName(IEnumerable<Permission> permissions)
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