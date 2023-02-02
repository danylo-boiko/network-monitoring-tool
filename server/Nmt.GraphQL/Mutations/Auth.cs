using Nmt.Core.CQRS.Commands.Auth.Login;
using Nmt.Core.CQRS.Commands.Auth.RefreshToken;
using Nmt.Core.CQRS.Commands.Auth.Register;
using Nmt.Core.CQRS.Commands.Auth.SendTwoFactorCode;
using Nmt.Core.CQRS.Commands.Auth.VerifyTwoFactorCode;
using Nmt.GraphQL.Consts;
using Nmt.GraphQL.Services.Interfaces;

namespace Nmt.GraphQL.Mutations;

[ExtendObjectType(ObjectTypes.Mutation)]
public class Auth
{
    public async Task<TokenDto> Login([Service] IExecutionResultService service, LoginCommand input)
    {
        return await service.HandleExecutionResultRequestAsync(input);
    }

    public async Task<bool> Register([Service] IExecutionResultService service, RegisterCommand input)
    {
        return await service.HandleExecutionResultRequestAsync(input);
    }

    public async Task<TokenDto> RefreshToken([Service] IExecutionResultService service, RefreshTokenCommand input)
    {
        return await service.HandleExecutionResultRequestAsync(input);
    }

    public async Task<bool> SendTwoFactorCode([Service] IExecutionResultService service, SendTwoFactorCodeCommand input)
    {
        return await service.HandleExecutionResultRequestAsync(input);
    }

    public async Task<bool> VerifyTwoFactorCode([Service] IExecutionResultService service, VerifyTwoFactorCodeCommand input)
    {
        return await service.HandleExecutionResultRequestAsync(input);
    }
}