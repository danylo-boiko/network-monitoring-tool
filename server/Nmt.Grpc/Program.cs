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
    .AddMediatR(typeof(MediatREntryPoint).Assembly)
    .ConfigureIdentity()
    .ConfigureJwt(configuration)
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