using MediatR;
using Nmt.Core;
using Nmt.Grpc.Services;
using Nmt.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddInfrastructure(configuration);
services.ConfigureIdentity();
services.AddMediatR(typeof(MediatREntryPoint).Assembly);
services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<AuthService>();
app.MapGrpcService<PacketsService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client.");

app.Run();