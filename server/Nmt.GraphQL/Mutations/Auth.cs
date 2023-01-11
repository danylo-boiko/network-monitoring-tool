using AppAny.HotChocolate.FluentValidation;
using Nmt.Core.CQRS.Commands.Auth.Login;
using Nmt.Core.CQRS.Commands.Auth.Register;
using Nmt.Core.CQRS.Commands.Auth.SendTwoFactorCode;
using Nmt.Core.CQRS.Commands.Auth.VerifyTwoFactorCode;
using Nmt.GraphQL.Consts;
using Nmt.GraphQL.Services.Interfaces;

namespace Nmt.GraphQL.Mutations;

[ExtendObjectType(ObjectTypes.Mutation)]
public class Auth
{
    public async Task<string> Login(
        [Service] IExecutionResultService executionResultService, 
        [UseFluentValidation] LoginCommand input)
    {
        return await executionResultService.HandleExecutionResultRequest(input);
    }

    public async Task<bool> Register(
        [Service] IExecutionResultService executionResultService, 
        [UseFluentValidation] RegisterCommand input)
    {
        return await executionResultService.HandleExecutionResultRequest(input);
    }
    
    public async Task<bool> SendTwoFactorCodeCommand(
        [Service] IExecutionResultService executionResultService, 
        [UseFluentValidation] SendTwoFactorCodeCommand input)
    {
        return await executionResultService.HandleExecutionResultRequest(input);
    }

    public async Task<bool> VerifyTwoFactorCodeCommand(
        [Service] IExecutionResultService executionResultService, 
        [UseFluentValidation] VerifyTwoFactorCodeCommand input)
    {
        return await executionResultService.HandleExecutionResultRequest(input);
    }
}