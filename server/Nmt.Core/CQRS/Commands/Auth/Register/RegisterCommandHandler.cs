using LS.Helpers.Hosting.API;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nmt.Core.CQRS.Commands.Auth.SendTwoFactorCode;
using Nmt.Domain.Consts;
using Nmt.Domain.Models;
using Nmt.Infrastructure.Data.Postgres;

namespace Nmt.Core.CQRS.Commands.Auth.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ExecutionResult<bool>>
{
    private readonly PostgresDbContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly IMediator _mediator;
    private readonly ILogger<RegisterCommandHandler> _logger;

    public RegisterCommandHandler(
        PostgresDbContext dbContext, 
        UserManager<User> userManager, 
        IMediator mediator, 
        ILogger<RegisterCommandHandler> logger)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<ExecutionResult<bool>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var isUsernameDuplicated = await _dbContext.Users.AnyAsync(u => u.UserName == request.Username, cancellationToken);
            if (isUsernameDuplicated)
            {
                return new ExecutionResult<bool>(new ErrorInfo(nameof(request.Username), "User with this username already exists"));
            }

            var isEmailDuplicated = await _dbContext.Users.AnyAsync(u => u.Email == request.Email, cancellationToken);
            if (isEmailDuplicated)
            {
                return new ExecutionResult<bool>(new ErrorInfo(nameof(request.Email), "User with this email already exists"));
            }

            var user = new User
            {
                UserName = request.Username,
                Email = request.Email
            };

            var userCreationResult = await _userManager.CreateAsync(user, request.Password);
            if (!userCreationResult.Succeeded)
            {
                var errors = userCreationResult.Errors
                    .Select(e => new ErrorInfo(e.Description))
                    .ToList();

                return new ExecutionResult<bool>(errors);
            }

            await _userManager.AddToRoleAsync(user, UserRoles.User);

            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return await _mediator.Send(new SendTwoFactorCodeCommand
            {
                Username = user.UserName
            }, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return new ExecutionResult<bool>(new ErrorInfo(e.Message));
        }
    }
}
