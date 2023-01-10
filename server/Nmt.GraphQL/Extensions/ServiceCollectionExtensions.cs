using AppAny.HotChocolate.FluentValidation;
using HotChocolate.Execution.Configuration;
using Nmt.GraphQL.Consts;
using Nmt.GraphQL.Mutations;
using Nmt.GraphQL.Queries;
using Nmt.GraphQL.Services;
using Nmt.GraphQL.Services.Interfaces;
using ExecutionResultExtensions = Nmt.Core.Extensions.ExecutionResultExtensions;

namespace Nmt.GraphQL.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<IExecutionResultService, ExecutionResultService>();

        return services;
    }

    public static IServiceCollection AddCors(this IServiceCollection services, IConfiguration configuration)
    {
        var clientUrl = configuration.GetValue<string>("ClientUrl")!;

        services.AddCors(opts =>
        {
            opts.AddDefaultPolicy(pb =>
            {
                pb.WithOrigins(clientUrl)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });

        return services;
    }

    public static IServiceCollection AddGraphQL(this IServiceCollection services)
    {
        services
            .AddGraphQLServer()
            .AddAuthorization()
            .AddErrorFilter<ErrorFilter>()
            .AddQueries()
            .AddMutations()
            .AddFluentValidation(vb => vb.UseErrorMapper((builder, context) =>
            {
                builder.SetMessage(context.ValidationFailure.ErrorMessage);
                builder.SetExtension(ExecutionResultExtensions.GraphQLPropertyName, context.ValidationFailure.PropertyName);
            }));

        return services;
    }

    private static IRequestExecutorBuilder AddQueries(this IRequestExecutorBuilder builder)
    {
        builder
            .AddQueryType(q => q.Name(ObjectTypes.Query))
            .AddType<Users>()
            .AddType<Packets>();

        return builder;
    }

    private static IRequestExecutorBuilder AddMutations(this IRequestExecutorBuilder builder)
    {
        builder
            .AddMutationType(q => q.Name(ObjectTypes.Mutation))
            .AddType<Auth>()
            .AddType<IpFilters>();

        return builder;
    }
}