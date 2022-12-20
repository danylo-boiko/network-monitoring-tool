using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

    public LoginCommandHandler(PostgresDbContext dbContext, SignInManager<User> signInManager, IMediator mediator)
    {
        _dbContext = dbContext;
        _signInManager = signInManager;
        _mediator = mediator;
    }

    public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == request.Username, cancellationToken);

        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), $"User with username '{request.Username}' not found");
        }
        
        var signInResult = await _signInManager.PasswordSignInAsync(user, request.Password, true, false);

        if (!signInResult.Succeeded)
        {
            throw new ArgumentException("Provided incorrect password");
        }

        return await _mediator.Send(new CreateTokenCommand
        {
            User = user
        }, cancellationToken);
    }
}

