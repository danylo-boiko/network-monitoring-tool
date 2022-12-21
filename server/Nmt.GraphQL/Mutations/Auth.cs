using MediatR;
using Nmt.Core.Commands.Auth;

namespace Nmt.GraphQL.Mutations;

public class Auth
{
    private readonly IMediator _mediator;

    public Auth(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<string> Login(LoginCommand input) => await _mediator.Send(input);
    
    public async Task<string> Register(RegisterCommand input) => await _mediator.Send(input);
}