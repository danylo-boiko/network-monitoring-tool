using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nmt.Core.CQRS.Queries.Users.GetUserWithDevicesAndIpFiltersById;
using Nmt.Core.Services.Interfaces;
using Nmt.Domain.Consts;
using Nmt.Domain.Events;
using Nmt.Domain.Exceptions;
using Nmt.Domain.Models;
using Nmt.Infrastructure.Data.Postgres;

namespace Nmt.Core.CQRS.Commands.Auth.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, TokenDto>
{
    private readonly PostgresDbContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly ITokensService _tokensService;
    private readonly IMediator _mediator;

    public LoginCommandHandler(
        PostgresDbContext dbContext,
        UserManager<User> userManager,
        ITokensService tokensService,
        IMediator mediator)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _tokensService = tokensService;
        _mediator = mediator;
    }

    public async Task<TokenDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
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

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
            {
                throw new DomainException("Incorrect password")
                {
                    Code = ExceptionCodes.WrongData,
                    Property = nameof(request.Password)
                };
            }

            if (!user.EmailConfirmed)
            {
                throw new DomainException("Email is not confirmed")
                {
                    Code = ExceptionCodes.EmailConfirmation
                };
            }

            var deviceId = await GetOrCreateDevice(user.Id, request.Hostname, request.MachineSpecificStamp, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            var accessToken = await _tokensService.CreateAccessTokenAsync(user.Id, deviceId, cancellationToken);

            return new TokenDto
            {
                AccessToken = accessToken,
                RefreshToken = _tokensService.CreateRefreshToken(accessToken)
            };
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
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

