using LS.Helpers.Hosting.API;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nmt.Core.CQRS.Commands.Auth.CreateToken;
using Nmt.Domain.Consts;
using Nmt.Domain.Models;
using Nmt.Infrastructure.Data.Postgres;

namespace Nmt.Core.CQRS.Commands.Auth.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, ExecutionResult<string>>
{
    private readonly PostgresDbContext _dbContext;
    private readonly SignInManager<User> _signInManager;
    private readonly IMediator _mediator;
    private readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(
        PostgresDbContext dbContext,
        SignInManager<User> signInManager,
        IMediator mediator,
        ILogger<LoginCommandHandler> logger)
    {
        _dbContext = dbContext;
        _signInManager = signInManager;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<ExecutionResult<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == request.Username, cancellationToken);
            if (user == null)
            {
                return new ExecutionResult<string>(new ErrorInfo(StatusCodes.NotFound, $"User with username '{request.Username}' not found"));
            }

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!signInResult.Succeeded)
            {
                return new ExecutionResult<string>(new ErrorInfo(StatusCodes.Unauthenticated, "Provided incorrect password"));
            }

            var deviceId = await GetOrCreateDevice(user.Id, request.Hostname, request.MachineSpecificStamp, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            var jwtToken =  await _mediator.Send(new CreateTokenCommand
            {
                UserId = user.Id,
                DeviceId = deviceId
            }, cancellationToken);

            return new ExecutionResult<string>(jwtToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return new ExecutionResult<string>(new ErrorInfo(e.Message));
        }
    }

    private async Task<Guid?> GetOrCreateDevice(Guid userId, string? hostname, string? machineSpecificStamp, CancellationToken cancellationToken)
    {
        if (hostname == null || machineSpecificStamp == null)
        {
            return null;
        }

        var device = await _dbContext.Devices.FirstOrDefaultAsync(d => d.UserId == userId && d.MachineSpecificStamp == machineSpecificStamp, cancellationToken);

        if (device == null)
        {
            device = new Device
            {
                UserId = userId,
                Hostname = hostname,
                MachineSpecificStamp = machineSpecificStamp,
                CreatedAt = DateTime.UtcNow
            };

            await _dbContext.Devices.AddAsync(device, cancellationToken);
        }

        return device.Id;
    }
}

