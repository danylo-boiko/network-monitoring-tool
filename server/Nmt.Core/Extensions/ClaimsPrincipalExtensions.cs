using System.Security.Claims;

namespace Nmt.Core.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid FindFirstGuid(this ClaimsPrincipal claimsPrincipal, string claimType)
    {
        var claimValueStr = claimsPrincipal.FindFirstValue(claimType);

        if (claimValueStr == null)
        {
            return Guid.Empty;
        }

        return Guid.TryParse(claimValueStr, out Guid claimValue) ? claimValue : Guid.Empty;
    }
}