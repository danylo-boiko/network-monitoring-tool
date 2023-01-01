using MediatR;
using Nmt.Core;
using Nmt.Core.Extensions;
using Nmt.GraphQL.Mutations;
using Nmt.GraphQL.Queries;
using Nmt.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services
    .AddInfrastructure(configuration)
    .AddMediatR(typeof(MediatREntryPoint).Assembly)
    .ConfigureIdentity()
    .ConfigureJwt(configuration)
    .AddGraphQLServer()
    .AddAuthorization()
    .AddQueryType<Users>()
    .AddMutationType<Auth>();

var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQL();

app.Run();