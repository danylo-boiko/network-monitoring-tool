using Microsoft.AspNetCore.Identity;
using Nmt.Domain.Models;

namespace Nmt.Core.TokenProviders;

public class TwoFactorCodeProvider : TotpSecurityStampBasedTokenProvider<User>
{
    public override Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<User> manager, User user)
    {
        return Task.FromResult(!user.EmailConfirmed);
    }
}