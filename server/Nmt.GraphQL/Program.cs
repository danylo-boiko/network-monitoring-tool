using MediatR;
using Nmt.Core;
using Nmt.Core.Extensions;
using Nmt.GraphQL;
using Nmt.GraphQL.Mutations;
using Nmt.GraphQL.Queries;
using Nmt.GraphQL.Services;
using Nmt.GraphQL.Services.Interfaces;
using Nmt.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services
    .AddInfrastructure(configuration)
    .AddTransient<IExecutionResultService, ExecutionResultService>()
    .AddMediatR(typeof(MediatREntryPoint).Assembly)
    .ConfigureJwt(configuration)
    .AddGraphQLServer()
    .AddAuthorization()
    .AddErrorFilter<ErrorFilter>()
    .AddQueryType(q => q.Name(ObjectTypes.Query))
    .AddType<Users>()
    .AddType<Packets>()
    .AddMutationType(q => q.Name(ObjectTypes.Mutation))
    .AddType<Auth>()
    .AddType<IpFilters>();

var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQL();

app.Run();