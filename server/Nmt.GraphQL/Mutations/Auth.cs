using MediatR;
using Nmt.Core.CQRS.Commands.Auth.Login;
using Nmt.Core.CQRS.Commands.Auth.Register;
using Nmt.Core.Extensions;

namespace Nmt.GraphQL.Mutations;

[ExtendObjectType(ObjectTypes.Mutation)]
public class Auth
{
    public async Task<string> Login([Service] IMediator mediator, LoginCommand input)
    {
        var jwtTokenResult = await mediator.Send(input);

        if (!jwtTokenResult.Success)
        {
            throw jwtTokenResult.ToGraphQLException();
        }

        return jwtTokenResult.Value;
    }

    public async Task<string> Register([Service] IMediator mediator, RegisterCommand input)
    {
        var jwtTokenResult = await mediator.Send(input);

        if (!jwtTokenResult.Success)
        {
            throw jwtTokenResult.ToGraphQLException();
        }

        return jwtTokenResult.Value;
    }
}