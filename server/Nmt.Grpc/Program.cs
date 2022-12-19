using Nmt.Grpc.Services;
using Nmt.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddInfrastructure(configuration);
services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<AuthService>();
app.MapGrpcService<PacketsService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client.");

app.Run();