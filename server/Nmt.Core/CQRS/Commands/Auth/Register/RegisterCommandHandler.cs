using LS.Helpers.Hosting.API;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nmt.Core.CQRS.Commands.Auth.CreateToken;
using Nmt.Domain.Consts;
using Nmt.Domain.Models;
using Nmt.Infrastructure.Data.Postgres;

namespace Nmt.Core.CQRS.Commands.Auth.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ExecutionResult<string>>
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

    public async Task<ExecutionResult<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var isUsernameDuplicated = await _dbContext.Users.AnyAsync(u => u.UserName == request.Username, cancellationToken);
            if (isUsernameDuplicated)
            {
                return new ExecutionResult<string>(new ErrorInfo($"User with username '{request.Username}' already exists"));
            }

            var isEmailDuplicated = await _dbContext.Users.AnyAsync(u => u.Email == request.Email, cancellationToken);
            if (isEmailDuplicated)
            {
                return new ExecutionResult<string>(new ErrorInfo($"User with email '{request.Email}' already exists"));
            }

            var user = new User
            {
                UserName = request.Username,
                Email = request.Email
            };

            var registerResult = await _userManager.CreateAsync(user, request.Password);
            if (!registerResult.Succeeded)
            {
                var err = registerResult.Errors.First();
                return new ExecutionResult<string>(new ErrorInfo(err.Code, err.Description));
            }

            await _userManager.AddToRoleAsync(user, UserRoles.User);

            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            var jwtToken = await _mediator.Send(new CreateTokenCommand
            {
                UserId = user.Id,
                DeviceId = null
            }, cancellationToken);

            return new ExecutionResult<string>(jwtToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return new ExecutionResult<string>(new ErrorInfo(e.Message));
        }
    }
}
