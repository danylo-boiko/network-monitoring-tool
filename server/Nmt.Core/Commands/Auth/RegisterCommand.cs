using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nmt.Domain.Models;
using Nmt.Infrastructure.Data.Postgres;

namespace Nmt.Core.Commands.Auth;

public record RegisterCommand : IRequest<string>
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, string>
{
    private readonly PostgresDbContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly IMediator _mediator;

    public RegisterCommandHandler(PostgresDbContext dbContext, UserManager<User> userManager, IMediator mediator)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _mediator = mediator;
    }
    
    public async Task<string> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var isUsernameDuplicated = await _dbContext.Users.AnyAsync(u => u.UserName == request.Username, cancellationToken);

        if (isUsernameDuplicated)
        {
            throw new ArgumentException($"User with username '{request.Username}' already exists");
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
            throw new Exception($"{err.Description}, {err.Code}");
        }

        return await _mediator.Send(new CreateTokenCommand
        {
            User = user
        }, cancellationToken);
    }
}
