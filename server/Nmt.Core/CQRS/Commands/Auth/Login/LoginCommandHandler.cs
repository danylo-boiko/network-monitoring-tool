using LS.Helpers.Hosting.API;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nmt.Core.CQRS.Commands.Auth.CreateToken;
using Nmt.Core.CQRS.Queries.Users.GetUserWithDevicesAndIpFiltersById;
using Nmt.Domain.Events;
using Nmt.Domain.Models;
using Nmt.Infrastructure.Data.Postgres;

namespace Nmt.Core.CQRS.Commands.Auth.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, ExecutionResult<string>>
{
    private readonly PostgresDbContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly IMediator _mediator;
    private readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(
        PostgresDbContext dbContext,
        UserManager<User> userManager,
        IMediator mediator,
        ILogger<LoginCommandHandler> logger)
    {
        _dbContext = dbContext;
        _userManager = userManager;
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
                return new ExecutionResult<string>(new ErrorInfo(nameof(request.Username), "User with this username not found"));
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
            {
                return new ExecutionResult<string>(new ErrorInfo(nameof(request.Password), "Incorrect password"));
            }

            if (!user.EmailConfirmed)
            {
                return new ExecutionResult<string>(new ErrorInfo(nameof(user.EmailConfirmed), "Email is not confirmed"));
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

        var deviceId = await _dbContext.Devices
            .Where(d => d.UserId == userId && d.MachineSpecificStamp == machineSpecificStamp)
            .Select(d => d.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (deviceId != Guid.Empty)
        {
            return deviceId;
        }

        var device = new Device
        {
            UserId = userId,
            Hostname = hostname,
            MachineSpecificStamp = machineSpecificStamp,
            CreatedAt = DateTime.UtcNow
        };

        await _dbContext.Devices.AddAsync(device, cancellationToken);

        await _mediator.Publish(new CacheInvalidated
        {
            Key = GetUserWithDevicesAndIpFiltersByIdQuery.GetCacheKey(device.UserId)
        }, cancellationToken);

        return device.Id;
    }
}

