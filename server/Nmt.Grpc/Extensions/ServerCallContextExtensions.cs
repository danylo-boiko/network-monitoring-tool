using System.Security.Claims;
using Grpc.Core;

namespace Nmt.Grpc.Extensions;

public static class ServerCallContextExtensions
{
    public static Guid GetAuthClaimValue(this ServerCallContext context, string claimType)
    {
        var claimValueStr = context.GetHttpContext().User.FindFirstValue(claimType);

        if (claimValueStr == null)
        {
            throw new RpcException(new Status(StatusCode.Unauthenticated, $"'{claimType}' missed in access token"));
        }

        if (!Guid.TryParse(claimValueStr, out Guid claimValue))
        {
            throw new RpcException(new Status(StatusCode.Unauthenticated, $"Impossible to parse '{claimType}'"));
        }

        return claimValue;
    }
}