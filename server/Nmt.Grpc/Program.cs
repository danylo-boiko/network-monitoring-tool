using MediatR;
using Nmt.Core;
using Nmt.Core.Extensions;
using Nmt.Grpc.Services;
using Nmt.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services
    .AddInfrastructure(configuration)
    .AddRedisCache()
    .AddMediatR(typeof(MediatREntryPoint).Assembly)
    .AddAuthentication(configuration)
    .AddGrpc();

var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcService<AuthService>();
app.MapGrpcService<PacketsService>();
app.MapGrpcService<IpFiltersService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client.");

app.Run();