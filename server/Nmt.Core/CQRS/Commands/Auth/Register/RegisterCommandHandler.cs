using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nmt.Core.CQRS.Commands.Auth.SendTwoFactorCode;
using Nmt.Domain.Consts;
using Nmt.Domain.Exceptions;
using Nmt.Domain.Models;
using Nmt.Infrastructure.Data.Postgres;

namespace Nmt.Core.CQRS.Commands.Auth.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, bool>
{
    private readonly PostgresDbContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly IMediator _mediator;

    public RegisterCommandHandler(
        PostgresDbContext dbContext, 
        UserManager<User> userManager, 
        IMediator mediator)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _mediator = mediator;
    }

    public async Task<bool> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var isUsernameDuplicated = await _dbContext.Users.AnyAsync(u => u.UserName == request.Username, cancellationToken);
            if (isUsernameDuplicated)
            {
                throw new DomainException("User with this username already exists")
                {
                    Code = ExceptionCodes.AlreadyExists,
                    Property = nameof(request.Username)
                };
            }

            var isEmailDuplicated = await _dbContext.Users.AnyAsync(u => u.Email == request.Email, cancellationToken);
            if (isEmailDuplicated)
            {
                throw new DomainException("User with this email already exists")
                {
                    Code = ExceptionCodes.AlreadyExists,
                    Property = nameof(request.Email)
                };
            }

            var user = new User
            {
                UserName = request.Username,
                Email = request.Email
            };

            var userCreationResult = await _userManager.CreateAsync(user, request.Password);
            if (!userCreationResult.Succeeded)
            {
                throw new Exception(string.Join(", ", userCreationResult.Errors.Select(e => e.Description)));
            }

            await _userManager.AddToRoleAsync(user, UserRoles.User);

            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return await _mediator.Send(new SendTwoFactorCodeCommand
            {
                Username = user.UserName
            }, cancellationToken);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
