using LS.Helpers.Hosting.API;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Nmt.Core.Services.Interfaces;
using Nmt.Core.TokenProviders;
using Nmt.Domain.Models;

namespace Nmt.Core.CQRS.Commands.Auth.SendTwoFactorCode;

public class SendTwoFactorCodeCommandHandler : IRequestHandler<SendTwoFactorCodeCommand, ExecutionResult<bool>>
{
    private readonly IEmailService _emailService;
    private readonly UserManager<User> _userManager;
    private readonly TwoFactorCodeProvider _twoFactorCodeProvider;
    
    public SendTwoFactorCodeCommandHandler(
        IEmailService emailService, 
        UserManager<User> userManager, 
        TwoFactorCodeProvider twoFactorCodeProvider)
    {
        _emailService = emailService;
        _userManager = userManager;
        _twoFactorCodeProvider = twoFactorCodeProvider;
    }
    
    public async Task<ExecutionResult<bool>> Handle(SendTwoFactorCodeCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return new ExecutionResult<bool>(new ErrorInfo(nameof(request.Email), "User with this email not found"));
        }

        if (user.EmailConfirmed)
        {
            return new ExecutionResult<bool>(new ErrorInfo(nameof(request.Email), "Your email is already confirmed"));
        }

        var code = await _twoFactorCodeProvider.GenerateAsync("registration", _userManager, user);

        await _emailService.SendMessageAsync(user.Email!, "Nmt two factor code", $"Your two factor code is {code}", cancellationToken);

        return new ExecutionResult<bool>(true);
    }
}