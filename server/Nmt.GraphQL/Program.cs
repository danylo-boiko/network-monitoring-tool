using MediatR;
using Nmt.Core;
using Nmt.Core.Extensions;
using Nmt.GraphQL.Extensions;
using Nmt.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services
    .AddInfrastructure(configuration)
    .AddRedisCache()
    .AddMediatR(typeof(MediatREntryPoint).Assembly)
    .AddAuthentication(configuration)
    .AddServices()
    .AddGraphQL();

var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQL();

app.Run();