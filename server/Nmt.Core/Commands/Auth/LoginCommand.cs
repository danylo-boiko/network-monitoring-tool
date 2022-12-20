using MediatR;

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
    public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        return "It's a token";
    }
}

