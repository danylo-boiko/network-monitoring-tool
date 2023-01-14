using LS.Helpers.Hosting.API;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nmt.Core.TokenProviders;
using Nmt.Domain.Models;
using Nmt.Infrastructure.Data.Postgres;

namespace Nmt.Core.CQRS.Commands.Auth.VerifyTwoFactorCode;

public class VerifyTwoFactorCodeCommandHandler : IRequestHandler<VerifyTwoFactorCodeCommand, ExecutionResult<bool>>
{
    private readonly UserManager<User> _userManager;
    private readonly TwoFactorCodeProvider _twoFactorCodeProvider;
    private readonly PostgresDbContext _dbContext;

    public VerifyTwoFactorCodeCommandHandler(
        UserManager<User> userManager, 
        TwoFactorCodeProvider twoFactorCodeProvider,
        PostgresDbContext dbContext)
    {
        _userManager = userManager;
        _twoFactorCodeProvider = twoFactorCodeProvider;
        _dbContext = dbContext;
    }

    public async Task<ExecutionResult<bool>> Handle(VerifyTwoFactorCodeCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == request.Username, cancellationToken);
        if (user == null)
        {
            return new ExecutionResult<bool>(new ErrorInfo(nameof(request.Username), "User with this username not found"));
        }

        if (user.EmailConfirmed)
        {
            return new ExecutionResult<bool>(new ErrorInfo(nameof(user.EmailConfirmed), "Your email is already confirmed"));
        }

        var isCodeValid = await _twoFactorCodeProvider.ValidateAsync("registration", request.TwoFactorCode, _userManager, user);
        if (!isCodeValid)
        {
            return new ExecutionResult<bool>(new ErrorInfo(nameof(request.TwoFactorCode),"Code and e-mail do not match"));
        }

        user.EmailConfirmed = true;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ExecutionResult<bool>(true);
    }
}