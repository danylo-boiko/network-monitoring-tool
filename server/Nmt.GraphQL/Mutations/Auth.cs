using MediatR;
using Nmt.Core.CQRS.Commands.Auth.Login;
using Nmt.Core.CQRS.Commands.Auth.RefreshToken;
using Nmt.Core.CQRS.Commands.Auth.Register;
using Nmt.Core.CQRS.Commands.Auth.SendTwoFactorCode;
using Nmt.Core.CQRS.Commands.Auth.VerifyTwoFactorCode;
using Nmt.GraphQL.Consts;

namespace Nmt.GraphQL.Mutations;

[ExtendObjectType(ObjectTypes.Mutation)]
public class Auth
{
    public async Task<TokenDto> Login([Service] IMediator mediator, LoginCommand input)
    {
        return await mediator.Send(input);
    }

    public async Task<bool> Register([Service] IMediator mediator, RegisterCommand input)
    {
        return await mediator.Send(input);
    }

    public async Task<TokenDto> RefreshToken([Service] IMediator mediator, RefreshTokenCommand input)
    {
        return await mediator.Send(input);
    }

    public async Task<bool> SendTwoFactorCode([Service] IMediator mediator, SendTwoFactorCodeCommand input)
    {
        return await mediator.Send(input);
    }

    public async Task<bool> VerifyTwoFactorCode([Service] IMediator mediator, VerifyTwoFactorCodeCommand input)
    {
        return await mediator.Send(input);
    }
}