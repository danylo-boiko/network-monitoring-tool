using LS.Helpers.Hosting.API;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nmt.Core.Services.Interfaces;
using Nmt.Core.TokenProviders;
using Nmt.Domain.Models;
using Nmt.Infrastructure.Data.Postgres;

namespace Nmt.Core.CQRS.Commands.Auth.SendTwoFactorCode;

public class SendTwoFactorCodeCommandHandler : IRequestHandler<SendTwoFactorCodeCommand, ExecutionResult<bool>>
{
    private readonly IEmailsService _emailsService;
    private readonly UserManager<User> _userManager;
    private readonly TwoFactorCodeProvider _twoFactorCodeProvider;
    private readonly PostgresDbContext _dbContext;

    public SendTwoFactorCodeCommandHandler(
        IEmailsService emailsService, 
        UserManager<User> userManager, 
        TwoFactorCodeProvider twoFactorCodeProvider,
        PostgresDbContext dbContext)
    {
        _emailsService = emailsService;
        _userManager = userManager;
        _twoFactorCodeProvider = twoFactorCodeProvider;
        _dbContext = dbContext;
    }

    public async Task<ExecutionResult<bool>> Handle(SendTwoFactorCodeCommand request, CancellationToken cancellationToken)
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

        var code = await _twoFactorCodeProvider.GenerateAsync("registration", _userManager, user);

        await _emailsService.SendEmailAsync(user.Email!, "NMT - Two factor code", $"Your two factor code: {code}", cancellationToken);

        return new ExecutionResult<bool>(true);
    }
}