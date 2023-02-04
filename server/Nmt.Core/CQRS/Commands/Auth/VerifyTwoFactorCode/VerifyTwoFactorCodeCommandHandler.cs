using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nmt.Core.TokenProviders;
using Nmt.Domain.Consts;
using Nmt.Domain.Exceptions;
using Nmt.Domain.Models;
using Nmt.Infrastructure.Data.Postgres;

namespace Nmt.Core.CQRS.Commands.Auth.VerifyTwoFactorCode;

public class VerifyTwoFactorCodeCommandHandler : IRequestHandler<VerifyTwoFactorCodeCommand, bool>
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

    public async Task<bool> Handle(VerifyTwoFactorCodeCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == request.Username, cancellationToken);
        if (user == null)
        {
            throw new DomainException("User with this username not found")
            {
                Code = ExceptionCodes.NotFound,
                Property = nameof(request.Username)
            };
        }

        if (user.EmailConfirmed)
        {
            throw new DomainException("Your email is already confirmed")
            {
                Code = ExceptionCodes.EmailConfirmation
            };
        }

        var isCodeValid = await _twoFactorCodeProvider.ValidateAsync("registration", request.TwoFactorCode, _userManager, user);
        if (!isCodeValid)
        {
            throw new DomainException("Code and e-mail do not match")
            {
                Code = ExceptionCodes.WrongData,
                Property = nameof(request.TwoFactorCode)
            };
        }

        user.EmailConfirmed = true;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}