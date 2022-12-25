using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nmt.Domain.Models;
using Nmt.Infrastructure.Data.Postgres;

namespace Nmt.Core.Commands.Auth;

public record LoginCommand : IRequest<string>
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Hostname { get; set; }
    public string MachineSpecificStamp { get; set; }
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
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

    public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == request.Username, cancellationToken);
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), $"User with username '{request.Username}' not found");
            }

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!signInResult.Succeeded)
            {
                throw new ArgumentException("Provided incorrect password");
            }

            var device = await _dbContext.Devices.FirstOrDefaultAsync(d => d.UserId == user.Id && d.MachineSpecificStamp == request.MachineSpecificStamp, cancellationToken);
            if (device == null)
            {
                device = new Device
                {
                    UserId = user.Id,
                    Hostname = request.Hostname,
                    MachineSpecificStamp = request.MachineSpecificStamp,
                    CreatedAt = DateTime.UtcNow
                };

                await _dbContext.Devices.AddAsync(device, cancellationToken);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return await _mediator.Send(new CreateTokenCommand
            {
                User = user,
                DeviceId = device.Id
            }, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return String.Empty;
        }
    }
}

