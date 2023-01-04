using Nmt.Core.CQRS.Commands.Auth.Login;
using Nmt.Core.CQRS.Commands.Auth.Register;
using Nmt.GraphQL.Services.Interfaces;

namespace Nmt.GraphQL.Mutations;

[ExtendObjectType(ObjectTypes.Mutation)]
public class Auth
{
    public async Task<string> Login([Service] IExecutionResultService executionResultService, LoginCommand input)
    {
        return await executionResultService.HandleExecutionResultRequest(input);
    }

    public async Task<string> Register([Service] IExecutionResultService executionResultService, RegisterCommand input)
    {
        return await executionResultService.HandleExecutionResultRequest(input);
    }
}