using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Nmt.Core.Auth;

public class PermissionsAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
{
    public PermissionsAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
    {
    }

    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        var policy = await base.GetPolicyAsync(policyName);

        if (policy != null)
        {
            return policy;
        }

        return new AuthorizationPolicyBuilder()
            .AddRequirements(new PermissionsRequirement(policyName))
            .Build();
    }
}