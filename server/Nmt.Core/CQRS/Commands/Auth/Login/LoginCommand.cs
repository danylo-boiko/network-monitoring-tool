using MediatR;

namespace Nmt.Core.CQRS.Commands.Auth.Login;

public record LoginCommand : IRequest<string>
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Hostname { get; set; }
    public string MachineSpecificStamp { get; set; }
}
