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
    private readonly IEmailService _emailService;
    private readonly UserManager<User> _userManager;
    private readonly TwoFactorCodeProvider _twoFactorCodeProvider;
    private readonly PostgresDbContext _dbContext;

    public SendTwoFactorCodeCommandHandler(
        IEmailService emailService, 
        UserManager<User> userManager, 
        TwoFactorCodeProvider twoFactorCodeProvider,
        PostgresDbContext dbContext)
    {
        _emailService = emailService;
        _userManager = userManager;
        _twoFactorCodeProvider = twoFactorCodeProvider;
        _dbContext = dbContext;
    }

    public async Task<ExecutionResult<bool>> Handle(SendTwoFactorCodeCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
        if (user == null)
        {
            return new ExecutionResult<bool>(new ErrorInfo(nameof(request.Email), "User with this email not found"));
        }

        if (user.EmailConfirmed)
        {
            return new ExecutionResult<bool>(new ErrorInfo(nameof(request.Email), "Your email is already confirmed"));
        }

        var code = await _twoFactorCodeProvider.GenerateAsync("registration", _userManager, user);

        await _emailService.SendEmailAsync(user.Email!, "NMT - Two factor code", $"Your two factor code: {code}", cancellationToken);

        return new ExecutionResult<bool>(true);
    }
}