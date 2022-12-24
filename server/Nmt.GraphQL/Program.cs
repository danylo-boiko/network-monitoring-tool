using MediatR;
using Nmt.Core;
using Nmt.Core.Extensions;
using Nmt.GraphQL.Mutations;
using Nmt.GraphQL.Queries;
using Nmt.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddInfrastructure(configuration);
services.AddMediatR(typeof(MediatREntryPoint).Assembly);
services.ConfigureIdentity();
services.ConfigureJwt(configuration);

services
    .AddGraphQLServer()
    .AddQueryType<Packets>()
    .AddMutationType<Auth>();

var app = builder.Build();

app.MapGraphQL();

app.Run();